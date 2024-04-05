using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace CHARACTERS
{
    public class Character_Sprite : Character
    {
        private const string RenderedSprParName = "Renderers";
        //spritesheet only
        private const char SPRITESHEET_DELIMITER = '-';
        //spritesheet only

        private CanvasGroup rootCG => root.GetComponent<CanvasGroup>();

        public List<CharacterSpriteLayer> layers = new List<CharacterSpriteLayer>();

        private string artAssetsDirectory = "";

        public Character_Sprite(string name, CharacterConfigData config, GameObject prefab, string rootAssetsFolder) : base(name, config, prefab)
        {
            rootCG.alpha = 0;
            artAssetsDirectory = rootAssetsFolder + "/Images";
            GetLayers();
        }

        private void GetLayers()
        {
            Transform renderedRoot = animator.transform.Find(RenderedSprParName);

            if (renderedRoot == null)
            {
                return;
            }

            for (int i = 0; i< renderedRoot.transform.childCount; i++)
            {
                Transform child = renderedRoot.transform.GetChild(i);
                Image rendererImage = child.GetComponent<Image>();

                if (rendererImage != null)
                {
                    CharacterSpriteLayer layer = new CharacterSpriteLayer(rendererImage, i);
                    layers.Add(layer);
                    child.name = $"Layer: {i}";
                }

            }
        }

        public void SetSprite(Sprite sprite, int layer = 0)
        {
            layers[layer].SetSprite(sprite);
        }

        public Sprite GetSprite(string spriteName)
        {
            if (config.characterType == CharacterType.SpriteSheet)
            {
                string[] data = spriteName.Split(SPRITESHEET_DELIMITER);
                Sprite[] spriteArray = new Sprite[0];
                if (data.Length == 2)
                {
                    string textureName = data[0];
                    spriteName = data[1];
                    spriteArray = Resources.LoadAll<Sprite>($"{artAssetsDirectory}/{textureName}");
                } else
                {
                    Debug.LogWarning("This shouldn't be happening! We do not have a default spritesheet implemented as of now. Please reformat the name of the spritesheet and try again!");
                    //go to visual novel ep 8 pt 1 if actually implementing a generic spritesheet
                }

                if (spriteArray.Length == 0)
                {
                    Debug.LogWarning("Please reformat your code so it looks like 'Sprite sprite_name = CHARACTER_NAME.GetSprite(\"Characters-your_sprite_name\")' and try again.");
                }
                return Array.Find(spriteArray, sprite => sprite.name == spriteName);
            } else
            {
                return Resources.Load<Sprite>($"{artAssetsDirectory}/{spriteName.Substring(1)}");
            }
        }

        public Coroutine TransitionSprite(Sprite sprite, int layer = 0, float speed = 1)
        {
            CharacterSpriteLayer spriteLayer = layers[layer];
            return spriteLayer.TransitionSprite(sprite, speed);
        }

        public override IEnumerator ShowingOrHiding(bool show, float speedMultiplier = 1f)
        {
            float targetAlpha = show ? 1f : 0;
            CanvasGroup self = rootCG;

            while (self.alpha != targetAlpha)
            {
                self.alpha = Mathf.MoveTowards(self.alpha, targetAlpha, 3f * Time.deltaTime * speedMultiplier);
                yield return null;
            }

            co_revealing = null;
            co_hiding = null;
        }

        public override void SetColor(Color color)
        {
            base.SetColor(color);
            color = displayColor;
            foreach (CharacterSpriteLayer layer in layers)
            {
                layer.StopChangingColor();
                layer.SetColor(color);
            }
        }

        public override IEnumerator ChangingColor(Color color, float speed)
        {
            foreach(CharacterSpriteLayer layer in layers)
            {
                layer.TransitionColor(color, speed);
            }
            yield return null;

            while (layers.Any(l => l.isChangingColor))
            {
                yield return null;
            }
            co_changingColor = null;
        }

        public override IEnumerator Highlighting(float speedMultiplier, bool immediate = false)
        {
            Color targetColor = displayColor;
            foreach(CharacterSpriteLayer layer in layers)
            {
                if (immediate)
                {
                    layer.SetColor(displayColor);
                } else
                {
                    layer.TransitionColor(targetColor, speedMultiplier);
                }
            }
            yield return null;
            while (layers.Any(l => l.isChangingColor))
            {
                yield return null;
            }
            co_highlighting = null;
        }

        public override void OnReceiveCastingExpression(int layer, string expression)
        {
            Sprite sprite = GetSprite(expression);
            if (sprite == null)
            {
                Debug.LogWarning($"Sprite '{expression}' could not be found for character '{name}'");
                return;
            }
            TransitionSprite(sprite, layer);
        }

        public override IEnumerator FaceDirection(bool faceLeft, float speedMultiplier, bool immediate)
        {
            foreach(CharacterSpriteLayer layer in layers)
            {
                if (faceLeft)
                {
                    layer.FaceLeft(speedMultiplier, immediate);
                } else
                {
                    layer.FaceRight(speedMultiplier, immediate);
                }
            }

            yield return null;

            while (layers.Any(l => l.isFlipping))
            {
                yield return null;
            }
            co_flipping = null;
        }
    }
}

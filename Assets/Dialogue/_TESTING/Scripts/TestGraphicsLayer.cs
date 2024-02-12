using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGraphicsLayer : MonoBehaviour
{
    private void Start()
    {
        GraphicPanel panel = GraphicPanelManager.instance.GetPanel("Background");
        GraphicLayer layer = panel.GetLayer(0, true);

        layer.SetTexture("Graphics/BG Images/2");
    }
}

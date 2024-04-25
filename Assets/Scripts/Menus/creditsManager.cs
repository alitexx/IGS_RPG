using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class creditsManager : MonoBehaviour
{
    //used to determine what ending text should pop up
    public static int endingID = 0;
    // 0 = everyone dead
    // 1 = only nicol alive
    // 2 = nicol and sophie alive
    // 3 = kisa alive
    // 4 = kisa and sophie alive
    // 5 = kisa and nicol alive
    // 6 = everyone alive!!
    [SerializeField] private TextMeshProUGUI endingText;
    [SerializeField] private RectTransform textScroll;
    [SerializeField] private RectTransform finalTextPos;
    [SerializeField] private GameObject[] charactersOnTitle;
    [SerializeField] private CanvasGroup fadeOutBG;
    [SerializeField] private CanvasGroup fadeInBG;
    [SerializeField] private CanvasGroup ReturnButton;
    [SerializeField] private audioManager audioManager;
    // 0 = kisa
    // 1 = nicol
    // 2 = sophie
    // 3 = alan
    [SerializeField] private Animator animator;

    IEnumerator runText()
    {
        // Turn on the animation controller
        animator.enabled = true;

        // Play the animation
        animator.Play("credits");

        // Wait until the animation completes
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return null;
        }

        // Disable the animation controller
        animator.enabled = false;
        textScroll.position = finalTextPos.position;

        fadeOutBG.DOFade(0, 5).OnComplete(() => {
            ReturnButton.DOFade(1, 1);
        });
    }


private void OnEnable()
    {
        switch (endingID)
        {
            case 0:// 0 = everyone dead
                charactersOnTitle[0].SetActive(false);
                charactersOnTitle[1].SetActive(false);
                charactersOnTitle[2].SetActive(false);
                endingText.text = "Alan found himself ensnared in the devil's grasp, a mere puppet dancing to the sinister tune of its commands to ensure Leora's continued " +
                    "existence. Stripped of his former self, he wandered the world as a hollow shell, devoid of the righteousness that once defined him. His heart, once filled " +
                    "with noble purpose, now lay barren, numb to the atrocities he committed in the devil's name. Though he bore the weight of his actions, he knew that he alone " +
                    "was responsible for the dire consequences that befell him. Amidst the darkness that consumed him, a fleeting semblance of joy flickered when Leora graced " +
                    "his presence, yet even this happiness remained hollow, unable to penetrate the emptiness in his soul.\r\n\r\nKisa, Nicol, Sophie... once cherished " +
                    "companions, now mere echoes in the recesses of Alan's fractured mind. Their names held no weight, their memories fading into obscurity under the weight of " +
                    "his guilt and desperation. Alan, consumed by his pact with darkness, ensured their absence went unnoticed by the world. They became nothing, lost to time, " +
                    "their stories silenced and condemned to oblivion.\r\n\r\nLeora's return was met with horror as she beheld the wretched transformation that had befallen Alan " +
                    "under the devil's influence. Her pleas for him to break free from the contract echoed in vain, for Alan had become a stranger, a shadow of his former self. " +
                    "As she grappled with the agonizing reality of his descent into darkness, Leora knew that the only recourse left was to confront him, to end the relentless " +
                    "cycle of soul - claiming that had ensnared him. With a heavy heart and steel resolve, she steeled herself for the inevitable confrontation, prepared to face " +
                    "the man she once knew in a battle for his very soul.";
                break;
            case 1:// 1 = only nicol alive
                charactersOnTitle[0].SetActive(false);
                charactersOnTitle[2].SetActive(false);
                endingText.text = "After triumphing over the lich, Alan seized control of its tower, enticed by the dark powers within. With the lich's spellbook in hand, he " +
                    "delved deeper into the forbidden arts, justifying his actions under the guise of 'justice'. Consumed by his obsession to resurrect his mentor, Alan " +
                    "remained convinced of the righteousness of his cause. As he prepared to claim the final soul required, he grew intoxicated by the raw power coursing " +
                    "through him. It was only a matter of time before whispers of his deeds reached the kingdom's ears, and another band of valiant adventurers would be " +
                    "called upon to confront the darkness that lurked within Alan's tower.\r\n\r\nKisa met her demise on the tower's first floor; This fact was confirmed " +
                    "when her family organized a search party after she failed to return for a month. Despite her shortcomings, she was remembered as a brave soul. Her family " +
                    "cherishes her memory through the lute she left behind.\r\n\r\nNicol's demeanor seemed to remain as carefree as ever, but he was deeply affected by the " +
                    "deaths of his comrades. His eyes darkened, his tone gentler and more longing than times prior. Nicol bestowed his fallen comrades with a solemn farewell, " +
                    "granting them a dignified burial. As he resumed his nomadic existence, he made regular pilgrimages to Isen, paying homage to their memory at their final " +
                    "resting place.\r\n\r\nSophie was absent when it came time to avenge her students.Her strength was undeniable, and many found it difficult to believe that " +
                    "the lich had claimed her. Meanwhile, her dojo faced closure and was undergoing transformation into a florist's shop. Sophie's family, unaware of her fate, " +
                    "assumed she was thriving in Isen, oblivious to her untimely demise.";
                break;
            case 2:// 2 = nicol and sophie alive
                charactersOnTitle[1].SetActive(false);
                charactersOnTitle[2].SetActive(false);
                endingText.text = "Despite continuing his life as a knight, Alan was haunted by the weight of his actions. Witnessing the profound impact of Kisa's death on " +
                    "the party, he swore never to harvest another soul. After his future epic adventures, the lich's tower gradually faded into distant memory.\r\n\r\nKisa met " +
                    "her demise on the tower's first floor; This fact was confirmed when her family organized a search party after she failed to return for a month. Despite " +
                    "her shortcomings, she was remembered as a brave soul. Her family cherishes her memory, but chooses to hand her precious lute off to her ally and close " +
                    "friend, Nicol.\r\n\r\nFollowing a simple burial ceremony for Kisa alongside Sophie and Kisa's relatives, Nicol resumed his life of adventure. He faithfully " +
                    "carried the lute that once resonated with his dear friend's melodies, its strings now a solemn echo of their shared memories on nights when solitude weighed " +
                    "heavily.\r\n\r\nWith the threat vanquished, Sophie found solace in the reopening of her dojo, yet the loss of Kisa lingered as a haunting presence. Paying " +
                    "homage to Kisa's memory, Sophie quietly honors her through subtle gestures, such as keeping a small memento of her in a cherished place within her dojo. " +
                    "Overwhelmed by guilt over Kisa's fate, she would sporadically close the dojo for days, wrestling with her inner turmoil. Despite this, Sophie persevered, " +
                    "drawing strength from the positive memories she had of Kisa.";
                break;
            case 3:// 3 = kisa alive
                charactersOnTitle[1].SetActive(false);
                charactersOnTitle[2].SetActive(false);
                charactersOnTitle[3].SetActive(false);
                endingText.text = "Alan was never seen again after the incident at the lich's tower.\r\n\r\nKisa was never seen again after the incident at the lich's tower." +
                    "Nicol's last known location was at the lich's tower. As a nomad by nature without any close " +
                    "ties, his absence went unnoticed by most except for the party he journeyed with. Although he left a lasting impression on those he encountered, his " +
                    "transient lifestyle caused him to gradually fade into obscurity.\r\n\r\nSophie was absent when it came time to avenge her students. Her strength was " +
                    "undeniable, and many found it difficult to believe that the lich had claimed her. Meanwhile, her dojo faced closure and was undergoing transformation " +
                    "into a florist's shop. Sophie's family, unaware of her fate, assumed she was thriving in Isen, oblivious to her untimely demise.";
                break;
            case 4:// 4 = kisa and sophie alive
                charactersOnTitle[1].SetActive(false);
                endingText.text = "Despite continuing his life as a knight, Alan was haunted by the weight of his actions. Witnessing the profound impact of Nicol's death on " +
                    "the party, he swore never to harvest another soul. After his future epic adventures, the lich's tower gradually faded into distant memory.\r\n\r\nFor " +
                    "years following Nicol's death, Kisa clung to a stubborn belief in his continued existence, despite Sophie's attempts to convince her otherwise. Then, in " +
                    "a sudden, crushing moment of clarity, the truth washed over her: Nicol was gone.Years of suppressed grief flooded her being, leaving her shattered. Seeking " +
                    "solace, she turned to the one person she trusted most.\r\n\r\nNicol's last known location was at the lich's tower. As a nomad by nature without any close " +
                    "ties, his absence went unnoticed by most except for the party he journeyed with.Although he left a lasting impression on those he encountered, his transient " +
                    "lifestyle caused him to gradually fade into obscurity.\r\n\r\nPaying homage to Nicol's memory, Sophie quietly honors him through subtle gestures, such as " +
                    "keeping a small memento of him in a cherished place within her dojo. She had resigned herself to the notion that she would never again see her former " +
                    "companions, until Kisa arrived at her doorstep seeking solace. Bound by their shared sorrow, they found comfort in each other's presence. Recognizing " +
                    "Kisa's need for guidance, Sophie offered her support, fostering a bond that endured for the remainder of their days.";
                break;
            case 5:// 5 = kisa and nicol alive
                charactersOnTitle[2].SetActive(false);
                endingText.text = "Despite continuing his life as a knight, Alan was haunted by the weight of his actions. Witnessing the profound impact of Sophie's death on " +
                    "the party, he swore never to harvest another soul. After his future epic adventures, the lich's tower gradually faded into distant memory.\r\n\r\nConsumed " +
                    "by guilt over Sophie's demise, Kisa found solace in confronting her emotions with Nicol's support. Accepting Nicol's offer to join his expeditions, she " +
                    "embarked on a journey of healing, both physically and emotionally, until her wounds had finally begun to mend.\r\n\r\nSensing Kisa's profound anguish in " +
                    "the wake of Sophie's passing, Nicol extended his hand in companionship. Welcoming Kisa's presence on their travels, he found comfort in their shared " +
                    "mourning, grateful for the camaraderie amid their grief.\r\n\r\nSophie was absent when it came time to avenge her students.Her strength was undeniable, " +
                    "and many found it difficult to believe that the lich had claimed her. Meanwhile, her dojo faced closure and was undergoing transformation into a florist's " +
                    "shop. Sophie's family, unaware of her fate, assumed she was thriving in Isen, oblivious to her untimely demise.";
                break;
            case 6:// 6 = everyone alive!!
                endingText.text = "After the events at the lich’s tower, Alan takes a much-needed break from his duties to prioritize his mental well-being. " +
                    "This time is short-lived, as Alan tends to find himself in the middle of conflict, fighting for the side of justice. He would go on many adventures, " +
                    "but unlike this quest, the temptation to revive his mentor had vanished. He would sometimes get wistful reminiscing about Leora, but he accepted the " +
                    "past was the past and it was time to move on.\r\n\r\nOnce their celebration concluded, Kisa ran home to tell her family of her journey. They were " +
                    "dismissive of her accomplishments and critical of her desire to continue the dangerous life of an adventurer. Kisa was furious, especially since she " +
                    "went through this ordeal to impress them. She ran away days later, choosing a life of excitement over a life of normalcy.\r\n\r\nNicol disappeared soon " +
                    "after the party’s celebration, continuing his carefree exploration of this world. He still treasures the bond he has with his companions, somehow winding " +
                    "up back in Isen for holidays to reunite with them. Despite all of his journey and experiences, Nicol never lost his childlike wonder for uncovering the " +
                    "secrets of the world\r\n\r\nSophie held a small funeral for her fallen students on her arrival home, still feeling a twinge of guilt; if only she knew " +
                    "of the danger awaiting them, she could have prevented their deaths. Twas a fleeting thought, as she knew she could not change the past. Sophie reopened " +
                    "her dojo and expected this to become a distant memory, yet she found herself reminiscing on the fond times with her companions. It was then that she knew " +
                    "how to respond to Kisa, and finally told Kisa the words she wanted to hear.";
                break;
        }
        audioManager.playBGM("T10");
        fadeInBG.DOFade(0, 2).OnComplete(() => {
            fadeInBG.gameObject.SetActive(false);
            StartCoroutine(runText());
        });
        
    }
    public void ReturnToTitle()
    {
        audioManager.playSFX(25);
        audioManager.stopBGM(2);
        fadeInBG.gameObject.SetActive(true);
        fadeInBG.DOFade(1, 2).OnComplete(() => {
            SceneManager.LoadScene("TitleScreen");
        });
    }

}

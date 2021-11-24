using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

/// <summary>
/// Dialogue Manager class manages dialogues by setting up the dialogue and displaying next sentences
/// </summary>
public class DialogueManager : MonoBehaviour
{
    public Text dialogueText;
    Queue<string> sentences = null;
    public Animator animator;
    public ObjDialogue dialogue;
    private bool wasContinued = false;
    public GameObject arrow;
    private Animator arrowAnim;
    public int count = 0;
    public int videoCount = 0;
    private bool animIsPlaying = false;
    private bool videoIsPlaying = false;
    public VideoPlayer videoPlayer;
    public void Start()
    {
     
        if(sentences == null)
            sentences = new Queue<string>();
        arrowAnim = arrow.GetComponent<Animator>();
    }

   

    /// <summary>
    /// StartDialogue function activates the animator object and calls GameManager to pause the game
    /// It starts the dialogue
    /// </summary>
    public void StartDialogue() 
    {
        animator.gameObject.SetActive(true);
        GameManager.instance.isDialogue = true;
        GameManager.instance.PauseGame();
        dialogue = new ObjDialogue();
        dialogue.AddSentences();
        animator.SetBool("IsOpen", true);
        sentences.Clear();
        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);         
        }
        DisplayNextSentence();
    }

    /// <summary>
    /// Almost the same function as StartDialogue() method
    /// It initializes the queue sentences
    /// </summary>
    public void StartDialogueForTheFirstTime()
    {
      if(sentences == null)
            sentences = new Queue<string>();
        arrowAnim = arrow.GetComponent<Animator>();
        StartDialogue();
        
    }

    /// <summary>
    /// Displays the next sentence from the queue
    /// If the next sentence is "#", it plays a specific video
    /// </summary>
    public void DisplayNextSentence()
    {

        if (animIsPlaying)
            EndAnim();
        if (videoIsPlaying)
            EndVideo();
        animator.SetBool("IsOpen", true);
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        
        string sentence = sentences.Dequeue();
        
        if (sentence == "\0")
        {
            wasContinued = true;
            EndDialogue();
            return;
        }

        if (sentence.StartsWith("#"))
        {
            
            GameManager.instance.Look();
            count++;
            
            DisplayNextSentence();
            StartAnim(count);
            if (sentence.Contains("*"))
            {
                StartVideo(videoCount);
                videoCount++;
            }
            return;

        }
        if (sentence.StartsWith("*"))
        {
            DisplayNextSentence();
            StartVideo(videoCount);
            videoCount++;
            return;

        }
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

    }

    /// <summary>
    /// Writes the sentence letter by letter
    /// </summary>
    /// <param name="sentence"></param>
    /// <returns></returns>
    IEnumerator TypeSentence(string sentence)
    {    
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
        
    }

    /// <summary>
    /// Calls GameManager to continue the game 
    /// </summary>
    void EndDialogue()
    {
        GameManager.instance.ContinueGame();
        GameManager.instance.isDialogue = false;
        animator.SetBool("IsOpen", false);
        
        
    }

    /// <summary>
    /// Starts the specific (num) arrow animation
    /// </summary>
    /// <param name="num"></param>
    private void StartAnim(int num)
    {
        arrowAnim.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load<RuntimeAnimatorController>("Animation/Arrow/arrow" + num);
        
        arrow.SetActive(true);
        animIsPlaying = true;
    }

    /// <summary>
    /// Plays the specific (num) video
    /// </summary>
    /// <param name="num"></param>
    private void StartVideo(int num)
    {
        videoPlayer.clip = (VideoClip)Resources.Load<VideoClip>("Video/Video" + num);
        videoPlayer.gameObject.SetActive(true);
        videoPlayer.Play();
        videoIsPlaying = true;
    }

    /// <summary>
    /// Stops the video
    /// </summary>
    private void EndVideo()
    {
        videoIsPlaying = false;
        videoPlayer.Stop();
        videoPlayer.clip = null;
        videoPlayer.gameObject.SetActive(false);
    }

    /// <summary>
    /// Stops the arrow animation
    /// </summary>
    private void EndAnim()
    {
        arrow.SetActive(false);
        animIsPlaying = false; 
    }

}

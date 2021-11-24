using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjDialogue
{
    public string name;
    public List<string> sentences = new List<string>();

    /// <summary>
    /// When called, based on the room in which the player currently is (GameManager.room), it returns sentences
    /// </summary>
    public void AddSentences()
    {
        switch (GameManager.instance.room)
        {
            case 0:
                sentences.Add("Hey! Wake up! I'll open the cell for you, but the rest is up to you...");
                sentences.Add("Move using WASD. Mouse rotates the camera and changes your view. By pressing Space key, you can jump.");
                break;
            case 1:            
                sentences.Add("Now, you  have to find a way to escape. There is no time to chat. But if you wonder, I am talking to you through your bracelet.");
                sentences.Add("#*");
                sentences.Add("You see the pillars right there? They are your way to freedom. Just walk between them.");
                //GameManager.instance.anim.GetComponent<Animator>().enabled = true;   
                GameManager.instance.SetTarget(GameManager.instance.pillarsLookAt);
                GameManager.instance.unblock.gameObject.SetActive(false);
                break;
            case 2:
                sentences.Add("Find the escap route");
                break;
            case 3:
                sentences.Add("While holding CTRL, you crouch.");
                break;
            case 4:
                sentences.Add("Okay, now that you are safe. You have been kindapped from your world. If you want to escape, you have to get through all these rooms.");
                sentences.Add("In three rooms, you will find three crystals, that will enable your escape.");
                sentences.Add("How to get through these rooms, you ask? Let me explain you some basics of our world.");
                sentences.Add("As you could have already noticed, in this world, there are two almost the same worlds that partly mirror. The Up-World and the Parallel-World.");
                sentences.Add("Because of security, some things can be done only in one of these worlds, but I will explain this later.");
                sentences.Add("#");
                GameManager.instance.SetTarget(GameManager.instance.doorLookAt);
                sentences.Add("Now about leaving this room... Do you see the open door in the other world? You have to get there. The portal will help, as you already know.");              
                break;
            case 5:
                sentences.Add("There are objects in both worlds. Some of them mirror and some of them don't.");
                sentences.Add("#");
                GameManager.instance.SetTarget(GameManager.instance.crateLookAt);
                sentences.Add("In the middle of the room, you can see a stone. This stone does mirror. It means that it reflects to the other world.");
                sentences.Add("#");
                sentences.Add("There is also a pressure plate that if triggered, opens the door to the next room.");
                sentences.Add("*");
                sentences.Add("You can step on the pressure plate, but if you step off the plate, the door does not stay opened. The stone might help with this.");
                sentences.Add("*");
                sentences.Add("But there is a problem. These mirroring stones can be moved only from the parallel world.");
                break;
            case 6:
                GameManager.instance.SetTarget(GameManager.instance.nonMirroringCrateLookAt);
                sentences.Add("#*");
                sentences.Add("There are also non-mirroring crates. You can move these crates in both worlds, but because they do not mirror to the other world, they affect only the world, where they are.");
                sentences.Add("Some doors require more objects to be triggered in order to open the door.");
                sentences.Add("The number of crystals next to the door indicates how many of these objects triggered together open the door.");
                
                break;
            case 7:
                sentences.Add("*");
                sentences.Add("Pressing 'B' while standing close to a crate, almost touching it, enables pulling.");
                break;
            case 8:
                GameManager.instance.SetTarget(GameManager.instance.barrierLookAt);
                sentences.Add("#");
                sentences.Add("As you can see, the door is open. But the passage is blocked by a barrier that needs to be disabled. The color of crystals on the pressure plates can help you identify what it might control.");
                sentences.Add("Look carefully at the other world. There is also some kind of a barrier right next to the stone. You cannot move the stone if something blocks its path in the other world, because the stone interacts with both wordls.");
                sentences.Add("At the same time, movable objects may move other movable objects. This could be helpful to know.");
                break;
            case 9:
                sentences.Add("Some things that seem to be the same in both worlds, may actually vary. They can be, for example, placed in different heights.");
                break;
            default:
                break;
        }
        
    }
}

using UnityEngine.Playables;
using UnityEngine.UI;

// A behaviour that is attached to a playable
public class PlayableTest : PlayableBehaviour
{
    public Text m_DialogContainer;
    public string m_DialogStr;

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (m_DialogContainer != null)
        {
            m_DialogContainer.gameObject.SetActive(true);
            m_DialogContainer.text = m_DialogStr;
        }
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (m_DialogContainer != null)
        {
            m_DialogContainer.gameObject.SetActive(false);
        }
    }
}


using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

[System.Serializable]
public class PlayableAssetTest : PlayableAsset
{
    public ExposedReference<Text> m_DialogContainer;
    public string m_DialogStr;

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<PlayableTest>.Create(graph);
        playable.GetBehaviour().m_DialogContainer = m_DialogContainer.Resolve(graph.GetResolver());
        playable.GetBehaviour().m_DialogStr = m_DialogStr;
        return playable;
    }
}

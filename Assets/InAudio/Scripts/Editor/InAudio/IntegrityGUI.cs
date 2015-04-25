using InAudioSystem;
using InAudioSystem.InAudioEditor;
using InAudioSystem.Internal;
using UnityEditor;
using UnityEngine;

public class IntegrityGUI
{
    public IntegrityGUI(InAudioBaseWindow window)
    {
    }

    public void OnEnable()
    {
           
    }

    public bool OnGUI()
    {
        EditorGUILayout.HelpBox("Do not Undo these operations! No guarantee about what could break.", MessageType.Warning);
        EditorGUILayout.Separator(); EditorGUILayout.Separator(); EditorGUILayout.Separator();
        EditorGUILayout.HelpBox("While the InAudio project hopefully is in perfect shape, bugs can happen. This will attempt to fix any problems.", MessageType.Info);
        if (GUILayout.Button("Fix integrity"))
        {
            FixParentChild();
            Debug.Log("Reassigned parent/childs");
            AudioBankWorker.RebuildBanks();
            Debug.Log("All Banks rebuild");
        }

        EditorGUILayout.Separator(); EditorGUILayout.Separator(); EditorGUILayout.Separator();

        EditorGUILayout.HelpBox("No nodes should be unused, but in the case there is this will remove all unused data.\nNo performance is lost if unused nodes remains, but it does waste a bit of memory. This will clean up any unused data", MessageType.Info);
        

        if (GUILayout.Button("Clean up unused data"))
        {
            DataCleanup.Cleanup();
        }
        return false;
    }

    private void FixParentChild()
    {
        var data = InAudioInstanceFinder.DataManager;
        TreeWalker.ForEach(data.AudioTree, node => node._getChildren.ForEach(n => n._parent = node));
        TreeWalker.ForEach(data.MusicTree, node => node._getChildren.ForEach(n => n._parent = node));
        TreeWalker.ForEach(data.BankLinkTree, node => node._getChildren.ForEach(n => n._parent = node));
        TreeWalker.ForEach(data.EventTree, node => node._getChildren.ForEach(n => n._parent = node));
    }

}

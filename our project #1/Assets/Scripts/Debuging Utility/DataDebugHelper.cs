using Sirenix.OdinInspector;
using UnityEngine;


public class DataDebugHelper : MonoBehaviour
{
    [InlineEditor, ShowIf("data"), ReadOnly]
    public UnityEngine.Object data;

    [ShowIf("stringData"), LabelText("data"), Multiline(),ReadOnly]
    public string stringData;
}

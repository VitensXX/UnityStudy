
using UnityEngine;

// 右键菜单
[CreateAssetMenu(menuName = "ScriptableObjects/CreateCubeScriptableObject")]
public class CubeScriptableObject : ScriptableObject
{
    public Vector3 position = default;
    public Vector3 rotation = default;
    public Vector3 scale = Vector3.one;
    public Color color = Color.white;
}

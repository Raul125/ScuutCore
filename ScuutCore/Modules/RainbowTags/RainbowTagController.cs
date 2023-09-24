namespace ScuutCore.Modules.RainbowTags;

using System;
using UnityEngine;

public sealed class RainbowTagController : MonoBehaviour
{
    private ServerRoles roles;
    private int position;
    private string[] colors;
    private float timeUntilNextUpdate;

    public string[] Colors
    {
        get => colors;
        set
        {
            colors = value ?? Array.Empty<string>();
            position = 0;
        }
    }

    public float Interval { get; set; }

    private void Awake()
    {
        roles = GetComponent<ServerRoles>();
    }

    private string RollNext()
    {
        if (++position >= colors.Length)
            position = 0;

        return colors.Length != 0 ? colors[position] : string.Empty;
    }

    private void Update()
    {
        timeUntilNextUpdate -= Time.deltaTime;
        if (timeUntilNextUpdate > 0f)
            return;
        timeUntilNextUpdate = Interval;
        string nextColor = RollNext();
        if (string.IsNullOrEmpty(nextColor))
            Destroy(this);
        else
            roles.SetColor(nextColor);
    }

}
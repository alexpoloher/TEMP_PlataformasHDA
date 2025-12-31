using UnityEngine;

public class GreenJeansController : MovementController
{

    // Update is called once per frame
    protected override void Update()
    {
        desiredMove = Vector2.left;
        base.Update();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSupplier : MonoBehaviour
{
    [SerializeField] private ObjectPoolProperties m_regBallPoolProps;

    [SerializeField] private ObjectPoolProperties m_goldenBallProps;

    [SerializeField] private float m_goldenBallChance;

    private ObjectPool<Rigidbody> m_regBallPool;

    private ObjectPool<Rigidbody> m_goldenBallPool;

    private void Awake()
    {
        m_regBallPool = new ObjectPool<Rigidbody>(m_regBallPoolProps);
    }

    public Rigidbody GetNewBall()
    {
        var ball = m_regBallPool.GetObjectComponent();

        return ball;
    }
}

using UnityEngine;

public class LaserSight : MonoBehaviour
{
    private const float MAX_LASER_LENGTH = 500.0f;

    [SerializeField] private LineRenderer   _lineRenderer;
    [SerializeField] private LayerMask      _laserHitMask;

    private Vector3     _laserStartPoint;
    private Vector3     _laserEndPoint;
    private RaycastHit  _laserHit;
    private float       _laserLength;

    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer.startWidth    = 0.02f;
        _lineRenderer.endWidth      = 0.02f;

        _laserStartPoint            = transform.position;
        _laserEndPoint              = Vector3.zero;
        _laserLength                = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {

        _laserStartPoint = this.transform.position;
        
        //_laserEndPoint = _laserStartPoint + this.transform.forward * 10.0f;

       

        if (Physics.Raycast(transform.position, transform.forward, out _laserHit, Mathf.Infinity, _laserHitMask))
        {
            if (_laserHit.collider)
            {
                _laserLength = _laserHit.distance;
            }
            else
            {
                _laserLength = MAX_LASER_LENGTH;
            }
        }
        else _laserLength = MAX_LASER_LENGTH;

        _laserEndPoint = _laserStartPoint + this.transform.forward * _laserLength;

        _lineRenderer.SetPosition(0, _laserStartPoint);
        _lineRenderer.SetPosition(1, _laserEndPoint);
    }
}

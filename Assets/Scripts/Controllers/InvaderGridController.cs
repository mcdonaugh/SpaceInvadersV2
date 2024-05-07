using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace SpaceInvadersV2.Controllers
{
    public class InvaderGridController : MonoBehaviour
    {
        private event Action OnRightBoundsHit;
        private event Action OnLeftBoundsHit;
        [SerializeField] private InvaderController _invader;
        [SerializeField] private int _maxRows = 5;
        [SerializeField] private int _maxColumns = 11;
        [SerializeField] private InvaderController[,] _invaderGrid;
        [SerializeField] private AudioClip[] _moveAudio;
        [SerializeField] private BoxCollider2D[] _gridColliders;
        [SerializeField] private float _moveSpeed = .031f;
        [SerializeField] private Vector2 _invaderSpawnOrigin;
        private Bounds _bounds;
        private int _currentRowIndex;
        private Vector3 _gridBoundsRight;
        private Vector3 _gridBoundsLeft;
        private AudioSource _audioSource;
        private int _moveAudioIndex;
        private bool _directionRight = true;
        private float _invaderGridOffset = .31f;
        private void Awake()
        {
            _invaderGrid = new InvaderController[_maxRows,_maxColumns];
            _invaderSpawnOrigin = new Vector2(-2f,0);
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_bounds.center, _bounds.size);
        }

        private void Update()
        {
            CalculateGridBounds();
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                InvaderMove();
            } 
        }

        public void GenerateInvaders()
        {
            GenerateInvaderGrid();
            StartCoroutine(SetInvaders());
            Debug.Log(_gridBoundsLeft.x);
        }

        private void GenerateInvaderGrid()
        {
            for (int i = 0; i < _maxRows; i++)
            {
                for (int j = 0; j < _maxColumns; j++)
                {
                    InvaderController newInvader = Instantiate(_invader, _invaderSpawnOrigin + new Vector2(j * _invaderGridOffset, i * _invaderGridOffset), quaternion.identity);
                    newInvader.SetCoordinates(i,j);
                    newInvader.OnInvaderDestroyed += OnInvaderDestroyedActionHandler;
                    newInvader.gameObject.SetActive(false);
                    _invaderGrid[i,j] = newInvader;

                    if (i < 1)
                    {
                        newInvader.SetShootStatus(true);
                    }
                }
            }
            OnRightBoundsHit += OnRightBoundsHitActionHandler;
        }

        private IEnumerator SetInvaders()
        {
            for (int i = 0; i < _maxRows; i++)
            {
                for (int j = 0; j < _maxColumns; j++)
                {
                    _invaderGrid[i,j].SetInvader(0);

                    if (i > 3)
                    {
                        _invaderGrid[i,j].SetInvader(2);
                    }
                    else if (i > 1)
                    {
                        _invaderGrid[i,j].SetInvader(1);
                    }

                    yield return new WaitForSeconds(.01f);
                    _invaderGrid[i,j].gameObject.SetActive(true);
                }
            }
        }

        private void InvaderMove()
        {
            if (_gridBoundsRight.x > 2.25)
            {
                OnRightBoundsHit?.Invoke();
            }
            if (_gridBoundsLeft.x < -2.25)
            {
                OnLeftBoundsHit?.Invoke();
            }
            else
            {
                StartCoroutine(InvaderMoveX());
            }
            PlayMoveAudio();
        }

        private IEnumerator InvaderMoveX()
        {
            for (int i = 0; i < _maxRows; i++)
            {
                for (int j = 0; j < _maxColumns; j++)
                {
                    yield return new WaitForSeconds(.01f);

                    if (_directionRight == true)
                    {
                        _invaderGrid[i,j].transform.position += new Vector3(_moveSpeed, transform.position.y, transform.position.z);
                    }
                    else if (_directionRight == false)
                    {
                        _invaderGrid[i,j].transform.position -= new Vector3(_moveSpeed, transform.position.y, transform.position.z);
                    }

                    _invaderGrid[i,j].PlayMoveAnimation();
                }
            }
        }

        private IEnumerator InvaderMoveY()
        {

            for (int i = 0; i < _maxRows; i++)
            {
                for (int j = 0; j < _maxColumns; j++)
                {
                    yield return new WaitForSeconds(.01f);
                    _invaderGrid[i,j].transform.position += new Vector3(transform.position.x, -_moveSpeed, transform.position.z);
                    _invaderGrid[i,j].PlayMoveAnimation();
                }
            }
        }

        private void OnRightBoundsHitActionHandler()
        {
            Debug.Log("Action Detected");
            StartCoroutine(InvaderMoveY());
            _directionRight = false;
            OnRightBoundsHit -= OnRightBoundsHitActionHandler;
            OnLeftBoundsHit += OnLeftBoundsHitActionHandler;
            
        }

        private void OnLeftBoundsHitActionHandler()
        {
            Debug.Log("Left Action Detected");
            StartCoroutine(InvaderMoveY());
            _directionRight = true;
            OnLeftBoundsHit -= OnLeftBoundsHitActionHandler;
            OnRightBoundsHit += OnRightBoundsHitActionHandler;
        }

        private void OnInvaderDestroyedActionHandler(InvaderController invader)
        {
            _invaderGrid[invader.InvaderRow, invader.InvaderCol].DestroyInvader();

            if (_invaderGrid[invader.InvaderRow, invader.InvaderCol].InvaderRow < _maxRows -1)
            {
                _invaderGrid[invader.InvaderRow, invader.InvaderCol].SetShootStatus(false);
                _invaderGrid[invader.InvaderRow + 1, invader.InvaderCol].SetShootStatus(true);
            }
        }

        private void PlayMoveAudio()
        {
            if (_moveAudioIndex >= _moveAudio.Length)
            {
                _moveAudioIndex = 0;
            }

            _audioSource.clip = _moveAudio[_moveAudioIndex];
            _audioSource.Play();
            _moveAudioIndex++; 
        }
        

        private void CalculateGridBounds()
        {
            _bounds = new Bounds();

            foreach (var invader in _invaderGrid)
            {
                if (invader.isActiveAndEnabled)
                {
                    Bounds newBounds = invader.GetComponent<Renderer>().bounds;
                    _bounds.Encapsulate(newBounds);
                    _gridBoundsRight = new Vector3(_bounds.max.x, transform.position.y, transform.position.z);
                    _gridBoundsLeft = new Vector3(_bounds.min.x, transform.position.y, transform.position.z);  
                }
            }
        }
    }    
}
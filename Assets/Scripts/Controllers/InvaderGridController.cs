using System.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace SpaceInvadersV2.Controllers
{
    public class InvaderGridController : MonoBehaviour
    {
        [SerializeField] private InvaderController _invader;

        [SerializeField] private int _maxRows = 5;
        [SerializeField] private int _maxColumns = 11;
        [SerializeField] private InvaderController[,] _invaderGrid;
        [SerializeField] private Vector2 _invaderSpawnOrigin;
        private float _invaderGridOffset = .31f;
        private float _invaderMoveInterval;
        [SerializeField] private float _moveDistance = .05f;
        private int _directionX = 1;
        private Bounds _bounds;
        private Vector3 _gridBoundsRight;
        private Vector3 _gridBoundsLeft;

        [SerializeField] private AudioClip[] _moveAudio;
        private AudioSource _audioSource;
        private int _moveAudioIndex;
        private int _invaderCount;

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

        public void GenerateInvaders()
        {
            GenerateInvaderGrid();
            StartCoroutine(SetInvaders());
            StartCoroutine(InvaderMoveTimer());
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
                    _invaderCount++;

                    if (i < 1)
                    {
                        newInvader.SetShootStatus(true);
                    }
                }
            }
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

        private IEnumerator InvaderMoveTimer()
        {
            CheckSpeed();

            while(_invaderGrid.Length > 0)
            {
                yield return new WaitForSeconds(_invaderMoveInterval);
                StartCoroutine(InvaderMove());
            }
        }

        private IEnumerator InvaderMove()
        {
            CalculateGridBounds();

            if (_gridBoundsRight.x >= 2.25)
            {
                _directionX = -1;
                InvaderMoveY();
                yield return new WaitForSeconds(1f);
            }
            
            if (_gridBoundsLeft.x <= -2.25)
            {
                _directionX = 1;
                InvaderMoveY();
                yield return new WaitForSeconds(1f);
            }

            InvaderMoveX();

            PlayMoveAudio();
        }

        private void InvaderMoveX()
        {
            for (int i = 0; i < _maxRows; i++)
            {
                for (int j = 0; j < _maxColumns; j++)
                {
                    _invaderGrid[i,j].transform.position += new Vector3(_directionX * _moveDistance, 0, 0);
                    _invaderGrid[i,j].PlayMoveAnimation();
                }
            }
        }

        private void InvaderMoveY()
        {
            for (int i = 0; i < _maxRows; i++)
            {
                for (int j = 0; j < _maxColumns; j++)
                {
                    _invaderGrid[i,j].transform.position -= new Vector3(0, _moveDistance, 0);
                    _invaderGrid[i,j].PlayMoveAnimation();
                }
            }
        }

        private void OnInvaderDestroyedActionHandler(InvaderController invader)
        {
            _invaderGrid[invader.InvaderRow, invader.InvaderCol].DestroyInvader();
            _invaderCount--;
            Debug.Log(_invaderCount);
            CheckSpeed();

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
                    Bounds newBounds = invader.GetComponent<Collider2D>().bounds;
                    _bounds.Encapsulate(newBounds);
                    _gridBoundsRight = new Vector3(_bounds.max.x, transform.position.y, transform.position.z);
                    _gridBoundsLeft = new Vector3(_bounds.min.x, transform.position.y, transform.position.z);  
                }
            }
        }
        
        private void CheckSpeed()
        {   
            _invaderMoveInterval = (float)_invaderCount/_invaderGrid.Length;
            Debug.Log(_invaderMoveInterval);
        }
    }    
}
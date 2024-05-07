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
        private float _invaderGridOffsetX = .3f;
        private float _invaderGridOffsetY = .35f;
        private float _invaderMoveInterval;
        [SerializeField] private float _moveDistance = .065f;
        private int _directionX = 1;
        private float _invaderMoveBounds;
        private Bounds _invaderGridBounds;
        private Vector2 _invaderGridBoundsRight;
        private Vector2 _invaderGridBoundsLeft;
        [SerializeField] private AudioClip[] _moveAudio;
        private AudioSource _audioSource;
        private int _moveAudioIndex;
        private int _invaderCount;

        private void Awake()
        {
            _invaderGrid = new InvaderController[_maxRows,_maxColumns];
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            CalculateGridBounds();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_invaderGridBounds.center, _invaderGridBounds.size);
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
                    InvaderController newInvader = Instantiate(_invader, _invaderSpawnOrigin + new Vector2(j * _invaderGridOffsetX, i * _invaderGridOffsetY), quaternion.identity);
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

        public void SetMoveBounds(float moveBounds)
        {
            _invaderMoveBounds = moveBounds;
        } 

        private IEnumerator InvaderMoveTimer()
        {
            CheckSpeed();

            while(_invaderGrid.Length > 0)
            {
                yield return new WaitForSeconds(_invaderMoveInterval);
                InvaderMove();
            }
        }

        private void InvaderMove()
        {

            if (_invaderGridBoundsRight.x >= _invaderMoveBounds)
            {
                _directionX = -1;
                InvaderMoveY();
            }
            
            if (_invaderGridBoundsLeft.x <= -_invaderMoveBounds)
            {
                _directionX = 1;
                InvaderMoveY();
            }

            
            StartCoroutine(InvaderMoveX());
            PlayMoveAudio();
        }

        private IEnumerator InvaderMoveX()
        {
            for (int i = 0; i < _maxRows; i++)
            {
                for (int j = 0; j < _maxColumns; j++)
                {
                    _invaderGrid[i,j].transform.position += new Vector3(_directionX * _moveDistance, 0, 0);
                    _invaderGrid[i,j].PlayMoveAnimation();
                    yield return new WaitForSeconds(_invaderMoveInterval * .01f);
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
            _invaderGridBounds = new Bounds();

            foreach (var invader in _invaderGrid)
            {
                if (invader.isActiveAndEnabled)
                {
                    Bounds newBounds = invader.GetComponent<Collider2D>().bounds;
                    _invaderGridBounds.Encapsulate(newBounds);
                    _invaderGridBoundsRight = new Vector2(_invaderGridBounds.max.x, transform.position.y);
                    _invaderGridBoundsLeft = new Vector2(_invaderGridBounds.min.x, transform.position.y);  
                }
            }
        }
        
        private void CheckSpeed()
        {   
            float newMoveInterval = (float)_invaderCount/_invaderGrid.Length;
            _invaderMoveInterval = newMoveInterval * .8f;
        }
    }    
}
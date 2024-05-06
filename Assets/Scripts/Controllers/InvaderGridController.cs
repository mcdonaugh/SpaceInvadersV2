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
        [SerializeField] private AudioClip[] _moveAudio;
        private AudioSource _audioSource;
        private int _moveAudioIndex;
        private Vector2 _invaderSpawnOrigin;
        private float _invaderGridOffset = .31f;
        private float _moveSpeed = .031f;
        private void Awake()
        {
            _invaderGrid = new InvaderController[_maxRows,_maxColumns];
            _invaderSpawnOrigin = new Vector2(-2f,0);
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                InvaderMove();
            }
        }
        public void GenerateInvaders()
        {
            GenerateInvaderGrid();
            StartCoroutine(SetInvaders());
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

        private void OnInvaderDestroyedActionHandler(InvaderController invader)
        {
            _invaderGrid[invader.InvaderRow, invader.InvaderCol].DestroyInvader();

            if (_invaderGrid[invader.InvaderRow, invader.InvaderCol].InvaderRow < _maxRows -1)
            {
                _invaderGrid[invader.InvaderRow, invader.InvaderCol].SetShootStatus(false);
                _invaderGrid[invader.InvaderRow + 1, invader.InvaderCol].SetShootStatus(true);
            }
        }

        private void InvaderMove()
        {
            StartCoroutine(InvaderMoveCo());
            PlayMoveAudio();
        }
        private IEnumerator InvaderMoveCo()
        {
            foreach (var invader in _invaderGrid)
            {
                yield return new WaitForSeconds(.01f);
                invader.transform.position += new Vector3(_moveSpeed,0,0);
                invader.PlayMoveAnimation();
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
    }    
}
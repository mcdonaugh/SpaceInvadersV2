using System;
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
        private Vector2 _invaderSpawnOrigin;
        private float _invaderGridOffset = .31f;
        private void Awake()
        {
            _invaderGrid = new InvaderController[_maxRows,_maxColumns];
            _invaderSpawnOrigin = new Vector2(-2f,0);
        }

        private void Start()
        {
            GenerateInvaderGrid();
        }

        private void GenerateInvaderGrid()
        {
            for (int i = 0; i < _maxRows; i++)
            {
                for (int j = 0; j < _maxColumns; j++)
                {
                    InvaderController newInvader = Instantiate(_invader, _invaderSpawnOrigin + new Vector2(j * _invaderGridOffset, i * _invaderGridOffset), quaternion.identity);
                    newInvader.SetInvaderCoordinates(i,j);
                    newInvader.OnInvaderDestroyed += OnInvaderDestroyedActionHandler;
                    _invaderGrid[i,j] = newInvader;
                    if (i < 1)
                    {
                        newInvader.SetShootStatus(true);
                    }
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
                Debug.Log($"Invader{invader.InvaderRow + 1},{_invader.InvaderCol} can now shoot"); 
            }
            
        }
    }    
}
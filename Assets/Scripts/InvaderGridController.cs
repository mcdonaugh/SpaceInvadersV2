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
        private float _invaderGridOffset = .3f;
        private void Awake()
        {
            _invaderGrid = new InvaderController[_maxRows,_maxColumns];
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
                    newInvader._invaderRow = i;
                    newInvader._invaderCol = j;
                    newInvader.OnInvaderDestroyed += OnInvaderDestroyedActionHandler;
                    _invaderGrid[i,j] = newInvader;
                    if (i < 1)
                    {
                        newInvader._canShoot = true;
                    }
                }
            }
        }

        private void OnInvaderDestroyedActionHandler(InvaderController invader)
        {
            _invaderGrid[invader._invaderRow, invader._invaderCol].gameObject.SetActive(false);
            _invaderGrid[invader._invaderRow, invader._invaderCol]._canShoot = false;
            _invaderGrid[invader._invaderRow + 1, invader._invaderCol]._canShoot = true;
            Debug.Log($"Invader{invader._invaderRow + 1},{_invader._invaderCol} can now shoot"); 
        }
    }    
}
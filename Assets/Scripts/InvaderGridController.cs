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
        private InvaderController[,] _invaderGrid;
        private float _gridOffset;
        private void Awake()
        {
            _invaderGrid = new InvaderController[_maxRows,_maxColumns];
        }

        private void Start()
        {
            GenerateInvaderGrid(_invader);
        }

        private void GenerateInvaderGrid(InvaderController invader)
        {
            for (int i = 0; i < _maxRows; i++)
            {
                for (int j = 0; j < _maxColumns; j++)
                {
                    InvaderController newInvader = Instantiate(invader, transform.position * new Vector2(j + _gridOffset, i + _gridOffset), quaternion.identity);
                    newInvader.OnInvaderDestroyed += OnInvaderDestroyedActionHandler;
                    newInvader.invaderRow = i;
                    newInvader.invaderCol = j;
                    _invaderGrid[i,j] = newInvader;     
                }
            }  
        }

        private void OnInvaderDestroyedActionHandler(InvaderController destroyedInvader)
        {
            Debug.Log($"Invader {destroyedInvader.invaderCol}, {destroyedInvader.invaderRow} Destroyed");
        }
    }    
}
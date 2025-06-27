using System;
using NaughtyAttributes;
using UnityEngine;

namespace DotsKiller.SaveSystem
{
    public class SaveLoadManager : MonoBehaviour
    {
        [SerializeField] private GameStateHandler gameStateHandler;
        [SerializeField] private bool active = true;
        [SerializeField] private float saveIntervalInSeconds = 5f;

        private SaveSerializer<GameState> _saveSerializer;

        private float _timeElapsed;


        private void Awake()
        {
            Initialize();
        }
        

        public void Initialize()
        {
            if (!active)
            {
                return;
            }

            _saveSerializer = new SaveSerializer<GameState>();

            Load();
        }


        private void Load()
        {
            GameState state = _saveSerializer.ReadFile();
            if (state == null)
            {
                return;
            }

            if (!state.IsDirty)
            {
                return;
            }

            GameStateHandler.Load(state);
        }


        private void Update()
        {
            if (!active)
            {
                return;
            }
            
            _timeElapsed += Time.deltaTime;
            if (_timeElapsed >= saveIntervalInSeconds)
            {
                Save();
                
                _timeElapsed = 0f;
            }
        }


        public void Save()
        {
            GameState state = GameStateHandler.State;
            state.IsDirty = true;
            state.LastSeen = DateTime.Now;
            _saveSerializer.SaveFile(state);
        }


        private void OnDestroy()
        {
            if (!active)
            {
                return;
            }

            //Save();
        }


        public void ClearSave()
        {
            gameStateHandler.Clear();
            
            GameState state = GameStateHandler.State;
            _saveSerializer.SaveFile(state);

            enabled = false;
        }

        
        [Button]
        public void DeleteSave()
        {
            SaveSerializer<GameState> saveSerializer = _saveSerializer ?? new SaveSerializer<GameState>();
            saveSerializer.DeleteFile();
            gameStateHandler.Clear();
        }


        public void DisableSavingForThisSession()
        {
            if (!active)
            {
                return;
            }

            active = false;
            _saveSerializer.DeleteFile();
        }
    }
}
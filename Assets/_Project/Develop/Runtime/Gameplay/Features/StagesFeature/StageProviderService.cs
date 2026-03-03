using Assets._Project.Develop.Runtime.Configs.Gameplay.Levels;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.StagesFeature
{
    public class StageProviderService : IDisposable
    {
        private ReactiveVariable<int> _currentStageNumber = new();
        private ReactiveVariable<StageResult> _currentStageResult = new();

        private LevelConfig _levelConfig;

        private StagesFactory _stagesFactory;

        private IStage _currentStage;

        private IDisposable _stageEndedDisposable;

        public StageProviderService(LevelConfig levelConfig, StagesFactory stagesFactory)
        {
            _levelConfig = levelConfig;
            _stagesFactory = stagesFactory;
        }

        public IReadOnlyVariable<int> CurrentStageNumber => _currentStageNumber;
        public IReadOnlyVariable<StageResult> CurrentStageResult => _currentStageResult;
        public int StagesCount => _levelConfig.StageConfigs.Count;

        public bool HasNextStage() => _currentStageNumber.Value < StagesCount;

        public void SwitchToNext()
        {
            if (HasNextStage() == false)
                throw new InvalidOperationException("Next stage is missing");

            if (_currentStage != null)
                CleanupCurrent();

            _currentStageNumber.Value++;
            _currentStageResult.Value = StageResult.Uncompleted;

            _currentStage = _stagesFactory.Create(_levelConfig.StageConfigs[_currentStageNumber.Value - 1]);

        }

        public void StartCurrent()
        {
            _stageEndedDisposable = _currentStage.Completed.Subscribe(OnStageCompleted);
            _currentStage.Start();
        }

        public void UpdateCurrent(float deltaTime) => _currentStage.Update(deltaTime);
        public void CleanupCurrent() => _currentStage.Cleanup();

        public void Dispose()
        {
            _stageEndedDisposable?.Dispose();
            _currentStage?.Dispose();
        }

        private void OnStageCompleted()
        {
            _currentStageResult.Value = StageResult.Completed;
        }
    }
}
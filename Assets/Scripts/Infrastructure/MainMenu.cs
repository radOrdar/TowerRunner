using System.Collections;
using System.Collections.Generic;
using Core.Loading;
using Infrastructure;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
   [SerializeField] private Button _continueBtn;
   [SerializeField] private Button _newGameBtn;

   private void Start()
   {
      _continueBtn.onClick.AddListener(OnContinueBtnClicked);
      _newGameBtn.onClick.AddListener(OnNewGameBtnClicked);
   }

   private void OnContinueBtnClicked()
   {
      DisableButtons();

      ProjectContext.I.LoadingScreenProvider.LoadAndDestroy(new GameLoadingOperation());
   }

   private void OnNewGameBtnClicked()
   {
      DisableButtons();
      
      Queue<ILoadingOperation> operations = new();
      operations.Enqueue(new ResetProgressOperation());
      operations.Enqueue(new GameLoadingOperation());
      
      ProjectContext.I.LoadingScreenProvider.LoadAndDestroy(operations);
   }

   private void DisableButtons()
   {
      _continueBtn.interactable = false;
      _newGameBtn.interactable = false;
   }
}

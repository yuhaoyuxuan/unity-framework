using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using System;

namespace Slf
{
    /**弹出类型 */
    public enum PopupType
    {
        None = 0,//没有效果
        MinToMax,//从小到大
        MaxToMin,//从大到小
        LeftToRight,//从左到右
        RightToLeft,//从右到左
        TopToBottom,//从上到下
        BottomToTop,//从下到上
    }

    //==========================
    // - Author:      slf         
    // - Date:        2021/07/14 15:07:20
    // - Description: 弹出效果
    //==========================
    public class UIPopup
    {
        /**
        * 添加弹出
        * @param ui 目标类
        */
        public void AddPopup(UIBase ui, Action complete = null)
        {
            TweenCallback action = () =>
            {
                if (complete != null)
                {
                    complete();
                }
            };
            PopupType type = ui.UiData.PopupType;
            if (type == PopupType.None)
            {
                action();
                return;
            }
            GameObject gameObject = ui.gameObject;
            float dV;
            switch (type)
            {
                case PopupType.MinToMax:
                case PopupType.MaxToMin:
                    dV = 0.1f;
                    if (type == PopupType.MaxToMin)
                    {
                        dV = 3.0f;
                    }
                    Vector3 startScale = new Vector3(dV, dV, dV);
                    Vector3 endScale = new Vector3(1, 1, 1);
                    ui.transform.localScale = startScale;
                    ui.transform.DOScale(endScale, 0.25f).SetEase(Ease.OutBack).OnComplete(action);
                    break;
                case PopupType.LeftToRight:
                case PopupType.RightToLeft:
                    dV = gameObject.transform.parent.transform.GetComponent<RectTransform>().sizeDelta.x;
                    if (type == PopupType.LeftToRight)
                    {
                        dV *= -1;
                    }
                    float startPos = dV;
                    float endPos = gameObject.transform.GetLocalPosX();
                    ui.transform.SetLocalPosX(startPos);
                    ui.transform.DOLocalMoveX(endPos, 0.25f).SetEase(Ease.InSine).OnComplete(action);
                    break;
                case PopupType.TopToBottom:
                case PopupType.BottomToTop:
                    dV = gameObject.transform.parent.transform.GetComponent<RectTransform>().sizeDelta.y;
                    if (type == PopupType.BottomToTop)
                    {
                        dV *= -1;
                    }
                    float startPos1 = dV;
                    float endPos1 = 0.0f;
                    ui.transform.SetLocalPosY(startPos1);
                    ui.transform.DOLocalMoveY(endPos1, 0.25f).SetEase(Ease.InSine).OnComplete(action);
                    break;
            }
        }
    }
}

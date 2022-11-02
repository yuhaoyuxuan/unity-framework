using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Slf
{

    //==========================
    // - Author:      slf         
    // - Date:        2021/10/13 16:18:02	
    // - Description: 扩展文本 设置文件固定宽度，大于宽度后 截断
    //==========================
    public class AText : Text
    {
        /// <summary>
        /// 文本宽度
        /// </summary>
        public int MaxWigth;
        /// <summary>
        /// 大于宽度 后缀省略字符
        /// </summary>
        public string Suffix = "...";

        private string oldStr = "";
        public override string text
        {
            get
            {
                return base.text;
            }
            set
            {
                oldStr = value;
                if (IsMaxWidth)
                {
                    base.text = GetWidthText() + Suffix;
                }
                else
                {
                    base.text = value;
                }
            }
        }

        /// <summary>
        /// 是否超宽
        /// </summary>
        public bool IsMaxWidth
        {
            get
            {
                if (MaxWigth == 0)
                {
                    return false;
                };
                return GetTextLength(oldStr) > MaxWigth;
            }
        }


        /// <summary>
        /// 获取字符串在text中的长度
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private int GetTextLength(string str = null)
        {
            Font mFont = font;
            mFont.RequestCharactersInTexture(str, fontSize, fontStyle);
            char[] charArr = str.ToCharArray();

            int totalTextLeng = 0;
            CharacterInfo character = new CharacterInfo();
            for (int i = 0; i < charArr.Length; i++)
            {
                mFont.GetCharacterInfo(charArr[i], out character, fontSize);
                totalTextLeng += character.advance;
            }
            return totalTextLeng;
        }

        private string GetWidthText()
        {
            int totalLength = 0;
            Font myFont = font;
            myFont.RequestCharactersInTexture(oldStr, fontSize, fontStyle);
            CharacterInfo characterInfo = new CharacterInfo();

            char[] charArr = oldStr.ToCharArray();

            int i = 0;

            int widthV = MaxWigth - GetTextLength(Suffix);

            for (; i < charArr.Length; i++)
            {
                myFont.GetCharacterInfo(charArr[i], out characterInfo, fontSize);

                int newLength = totalLength + characterInfo.advance;
                if (newLength > widthV)
                {
                    if (Mathf.Abs(newLength - widthV) > Mathf.Abs(widthV - totalLength))
                    {
                        break;
                    }
                    else
                    {
                        totalLength = newLength;
                        break;
                    }
                }
                totalLength += characterInfo.advance;
            }
            return oldStr.Substring(0, i);
        }
    }

}

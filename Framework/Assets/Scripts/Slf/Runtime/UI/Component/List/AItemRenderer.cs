namespace Slf
{

    /// <summary>
    /// 单项渲染接口
    /// </summary>
    public interface IAItemRenderer
    {
        object SubData
        {
            get;
            set;
        }
        //数据发生变化 刷新ui显示 子项重写
        void DataChange();
    }


    //==========================
    // - Author:      slf         
    // - Date:        2021/09/14 14:28:16	
    // - Description: 单项渲染
    //==========================
    public class AItemRenderer<T> : AMonoBehaviour, IAItemRenderer
    {
        protected T _Data;
        public T Data
        {
            get { return _Data; }
            set { _Data = value; DataChange(); }
        }

        public object SubData
        {
            get { return Data; }
            set { Data = (T)value; }
        }

        //数据发生变化 刷新ui显示 子项重写
        public virtual void DataChange() { }
    }
}

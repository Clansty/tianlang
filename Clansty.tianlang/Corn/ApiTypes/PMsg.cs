using System;
using System.Collections.Generic;
using System.Text;

namespace CornSDK.ApiTypes
{
    class PMsg
    {
        /// <summary>
        /// 
        /// </summary>
        public int Req { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long Seq { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int SubType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int SubTempType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int BubbleID { get; set; }
    }
}

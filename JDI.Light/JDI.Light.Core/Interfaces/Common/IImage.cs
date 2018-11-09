﻿using JDI.Core.Interfaces.Base;

namespace JDI.Core.Interfaces.Common
{
    public interface IImage : IClickable
    {
        /**
        * @return Get image source
        */
        //TODO[JDIAction]
        string GetSource();

        /**
         * @return Get image alt/hint text
         */
        //TODO[JDIAction]
        string GetAlt();
    }
}
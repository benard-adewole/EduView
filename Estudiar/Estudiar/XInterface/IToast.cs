using System;
using System.Collections.Generic;
using System.Text;

namespace Estudiar.XInterface
{
    public interface IToast
    {
        void ShortToast(string str);
        void LongToast(string str);
    }
    public interface ISnackBar
    {
        void SnackbarShow(string str);
    }
}

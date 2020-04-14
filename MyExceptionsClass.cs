using System;
using System.Runtime.Serialization;

namespace ExceptionHandling
{
    [Serializable]
    internal class MyFirstExceptionsClass : Exception
    {
        public int Value { get; }
        public MyFirstExceptionsClass() { }
        public MyFirstExceptionsClass(string message) : base(message) { }      
        public MyFirstExceptionsClass(string message, Exception inner) : base(message, inner) { }
        protected MyFirstExceptionsClass(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public MyFirstExceptionsClass(string message, int val) : base(message)
        {
            Value = val;
        }
    }
    [Serializable]
    internal class MyObjectInitializationException : ApplicationException, ISerializable
    {
        private string _objInitError;
        public string ObjInitError
        {
            get
            {
                return _objInitError;
            }
            set
            {
                _objInitError = value;
            }
        }
        public MyObjectInitializationException() { }
        public MyObjectInitializationException(string message) : base(message) { }
        public MyObjectInitializationException(string message, Exception inner) : base(message, inner) { }
        protected MyObjectInitializationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            if (info != null)
            {
                this._objInitError = info.GetString("ObjInitError");
            }
        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("ObjInitError", this.ObjInitError);
        }
    }
    [Serializable]
    internal class MyInvalidCastExceptionClass : InvalidCastException, ISerializable
    {
        /*Выбрасывается, когда явное преобразование
          базового типа или интерфейса в унаследованный
          тип во время выполнения программы завершается
          неудачей*/
        private string _invalidCastError;
        public string InvalidCastError
        {
            get
            {
                return _invalidCastError;
            }
            set
            {
                _invalidCastError = value;
            }
        }
        public MyInvalidCastExceptionClass() { }
        public MyInvalidCastExceptionClass(string message) : base(message) { }
        public MyInvalidCastExceptionClass(string message, Exception inner) : base(message, inner) { }
        protected MyInvalidCastExceptionClass(SerializationInfo info, StreamingContext context) : base(info, context) 
        {
            if (info != null)
            {
                this._invalidCastError = info.GetString("InvalidCastError");
            }
        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("InvalidCastError", this.InvalidCastError);
        }
    }
    [Serializable]
    internal class MyNullReferenceExceptionClass : NullReferenceException, ISerializable
    {
        /*Выбрасывается, когда ссылка, имеющая значение
          null, используется таким образом, при котором
          требуется обращение к объекту, на который она
          должна ссылаться*/
        private string _nullReferenceError;
        public string NullReferenceError
        {
            get
            {
                return _nullReferenceError;
            }
            set
            {
                _nullReferenceError = value;
            }
        }
        public MyNullReferenceExceptionClass() { }
        public MyNullReferenceExceptionClass(string message) : base(message) { }
        public MyNullReferenceExceptionClass(string message, Exception inner) : base(message, inner) { }
        protected MyNullReferenceExceptionClass(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            if (info != null)
            {
                this._nullReferenceError = info.GetString("NullReferenceError");
            }
        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("NullReferenceError", this.NullReferenceError);
        }
    }
}

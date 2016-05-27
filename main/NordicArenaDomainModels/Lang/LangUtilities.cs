using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NordicArenaDomainModels.Lang
{
    /// <summary>
    /// Diverse helper methods for simplifying common C#/.NET tasks
    /// </summary>
    public static class LangUtilities
    {
        public static TOut GetShallowCloneByReflection<TOut>(this Object objIn, Type inType = null)
        {
            TOut objOut = (TOut)Activator.CreateInstance(typeof(TOut));
            objIn.CopyPropertiesTo(objOut, inType);
            return objOut;
        }

        /// <summary>
        /// Copies all properties and fields from objIn to objOut. 
        /// </summary>
        /// <param name="objIn"></param>
        /// <param name="objOut"></param>
        /// <param name="inType">If specified, overrides the type of objIn. Beware of runtime errors</param>
        /// <param name="includeInheritedProperties"></param>
        public static void CopyPropertiesTo(this Object objIn, Object objOut, Type inType = null, bool includeInheritedProperties = false)
        {
            Type inputType = inType == null ? objIn.GetType() : inType;
            Type outputType = objOut.GetType();
            if (!outputType.Equals(inputType) && !outputType.IsSubclassOf(inputType)) throw new ArgumentException(String.Format("{0} is not a subclass of {1}", outputType, inputType));
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            if (includeInheritedProperties) flags |= BindingFlags.FlattenHierarchy;
            PropertyInfo[] properties = inputType.GetProperties(flags);
            FieldInfo[] fields = inputType.GetFields(flags);
            foreach (PropertyInfo property in properties)
            {
                try
                {
                    property.SetValue(objIn, property.GetValue(objIn, null), null);
                }
                catch (ArgumentException) { } // For Get-only-properties
            }
            foreach (FieldInfo field in fields)
            {
                field.SetValue(objOut, field.GetValue(objIn));
            }
        }

        private static void SetProperty(PropertyInfo Property, object ClassInstance, object Object)
        {
            try
            {
                Property.SetValue(ClassInstance, Property.GetValue(Object, null), null);
            }
            catch (ArgumentException) { } // Set method not existing causes this                
        }

        /// <summary>
        /// Shuffles a list randomly
        /// </summary>
        /// <param name="list"></param>
        /// <param name="randomGenerator">Optional initialized random generator</param>
        /// <returns></returns>
        public static void Shuffle<T>(this IList<T> list, Random randomGenerator = null)
        {
            if (randomGenerator == null) randomGenerator = new Random();
            int initialCount = list.Count;
            var targetList = new List<T>();
            for (int i = 0; i < initialCount; i++)
            {
                int ix = randomGenerator.Next(0, list.Count);
                T obj = list[ix];
                list.RemoveAt(ix);
                targetList.Add(obj);
            }
            foreach (var elem in targetList) list.Add(elem);
        }

        /// <summary>
        /// Returns a default value of type U if the key does not exist in the dictionary        
        /// </summary>
        /// <param name="dic">The dictionary to search</param>
        /// <param name="key">Key to search for</param>
        /// <param name="onMissing">Optional default value of type U. If not specified, the C# default value will be returned.</param>
        public static U GetOrDefault<T, U>(this Dictionary<T, U> dic, T key, U onMissing = default(U))
        {
            U value;
            return dic.TryGetValue(key, out value) ? value : onMissing;
        }

        /// <summary>
        /// Returns an existing value U for key T, or creates a new instance of type U using the default constructor, 
        /// adds it to the dictionary and returns it.
        /// </summary>
        public static U GetOrInsertNew<T, U>(this Dictionary<T, U> dic, T key)
            where U : new()
        {
            if (dic.ContainsKey(key)) return dic[key];
            U newObj = new U();
            dic[key] = newObj;
            return newObj;
        }

        public static String TextWriterToString(Action<TextWriter> swAction)
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                swAction(sw);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns the name of the method of the expression provided
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static String MethodNameFor(Expression<Action> exp)
        {
            return ((MethodCallExpression)exp.Body).Method.Name;
        }

        public static String MethodNameFor<T>(Expression<Func<T>> exp)
        {
            return ((MethodCallExpression)exp.Body).Method.Name;
        }

        public static String MethodNameFor<T, U>(Expression<Func<T, U>> exp)
        {
            return ((MethodCallExpression)exp.Body).Method.Name;
        }

        /// <summary>
        /// Returns true if not-null and Value==true
        /// </summary>
        public static bool IsTrue(this bool? b) 
        {
            return b != null && b.Value;
        }

        /// <summary>
        /// Returns the number of decimals in the number. 
        /// Only works for numbers with 18 digits or less.
        /// </summary>
        public static int GetDecimalCount(this decimal number)
        {
            if (number.ToString().Length > 18) throw new ArgumentException("Number too large");
            int count = 0;
            while (number != (decimal)((long)number))
            {
                number *= 10;
                count++;
            }
            return count;
        }

        public static bool AreInRisingOrder<T>(this IEnumerable<T> someEnumerable, Func<T, IComparable> someProperty)
        {
            IComparable previous = null;
            foreach (T entity in someEnumerable)
            {
                IComparable comparable = someProperty.Invoke(entity);
                if (previous != null)
                {
                    if (previous.CompareTo((comparable)) > 0) return false;
                }
                previous = comparable;
            }
            return true;
        }
    }
}
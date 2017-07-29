using System;
using System.Collections.Generic;
using System.Diagnostics;
using Model.JobLanes.Extensions;

namespace Model.JobLanes.Helper
{
    public class Check
    {
        internal Check()
        { }

        public class Argument
        {
            internal Argument()
            { }

            #region Guid

            [DebuggerStepThrough]
            public static void Empty(Guid argument, string argumentName)
            {
                if (argument == Guid.Empty)
                {
                    throw new ArgumentException("\"{0}\" cannot be empty guid.".FormatWith(argumentName), argumentName);
                }
            }

            #endregion

            #region String

            [DebuggerStepThrough]
            public static void EmptyOrWhiteSpace(string argument, string argumentName)
            {
                if (string.IsNullOrWhiteSpace((argument ?? string.Empty).Trim()))
                {
                    throw new ArgumentNullException("\"{0}\" cannot be blank.".FormatWith(argumentName), argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void EmptyOrWhiteSpace(string argument, string argumentName, string message)
            {
                if (string.IsNullOrWhiteSpace((argument ?? string.Empty).Trim()))
                {
                    throw new ArgumentNullException(message, argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void OutOfLength(string argument, int length, string argumentName)
            {
                if (argument.Trim().Length > length)
                {
                    throw new ArgumentException("\"{0}\" cannot be more than {1} character.".FormatWith(argumentName, length), argumentName);
                }
            }

            #endregion

            #region Object

            [DebuggerStepThrough]
            public static void Null(object argument, string argumentName)
            {
                if (argument == null)
                {
                    throw new ArgumentNullException(argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void Null(object argument, string argumentName, string message)
            {
                if (argument == null)
                {
                    throw new ArgumentNullException(argumentName, message);
                }
            }

            #endregion

            #region Number

            [DebuggerStepThrough]
            public static void Negative(int argument, string argumentName)
            {
                if (argument < 0)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void NegativeOrZero(int argument, string argumentName)
            {
                if (argument <= 0)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void Negative(long argument, string argumentName)
            {
                if (argument < 0)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void NegativeOrZero(long argument, string argumentName)
            {
                if (argument <= 0)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void Negative(float argument, string argumentName)
            {
                if (argument < 0)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void NegativeOrZero(float argument, string argumentName)
            {
                if (argument <= 0)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void Negative(decimal argument, string argumentName)
            {
                if (argument < 0)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void NegativeOrZero(decimal argument, string argumentName)
            {
                if (argument <= 0)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void OutOfRange(int argument, int min, int max, string argumentName)
            {
                if ((argument < min) || (argument > max))
                {
                    throw new ArgumentOutOfRangeException(argumentName, "{0} must be between \"{1}\"-\"{2}\".".FormatWith(argumentName, min, max));
                }
            }

            #endregion

            #region Date

            [DebuggerStepThrough]
            public static void InvalidDate(DateTime argument, string argumentName)
            {
                if (!argument.IsValid())
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void InPast(DateTime argument, string argumentName)
            {
                if (argument < DateTime.UtcNow)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void InFuture(DateTime argument, string argumentName)
            {
                if (argument > DateTime.UtcNow)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void Negative(TimeSpan argument, string argumentName)
            {
                if (argument < TimeSpan.Zero)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void NegativeOrZero(TimeSpan argument, string argumentName)
            {
                if (argument <= TimeSpan.Zero)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }

            #endregion

            #region Collection

            [DebuggerStepThrough]
            public static void Empty<T>(ICollection<T> argument, string argumentName)
            {
                Null(argument, argumentName);

                if (argument.Count == 0)
                {
                    throw new ArgumentException("Collection cannot be empty.", argumentName);
                }
            }

            #endregion

            #region Email and Web

            [DebuggerStepThrough]
            public static void InvalidEmail(string argument, string argumentName)
            {
                EmptyOrWhiteSpace(argument, argumentName);

                if (!argument.IsEmail())
                {
                    throw new ArgumentException("\"{0}\" is not a valid email address.".FormatWith(argumentName), argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void InvalidWebUrl(string argument, string argumentName)
            {
                EmptyOrWhiteSpace(argument, argumentName);

                if (!argument.IsWebUrl())
                {
                    throw new ArgumentException("\"{0}\" is not a valid web url.".FormatWith(argumentName), argumentName);
                }
            }

            #endregion
        }
    }
}
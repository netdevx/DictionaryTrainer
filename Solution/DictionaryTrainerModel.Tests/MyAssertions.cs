using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DictionaryTrainerModel.Tests
{
    public static class MyAssertions
    {
        public static void CollectionAssert<T, T2>(IEnumerable<T> expectedCollection, IEnumerable<T2> actualCollection, Func<T, T2, bool> elementPredicate)
        {
            if (expectedCollection.Count() != actualCollection.Count())
                throw new AssertFailedException("Sizes of collections are not equal!");

            IEnumerator<T> expectedEnumerator = expectedCollection.GetEnumerator();
            IEnumerator<T2> actualEnumerator = actualCollection.GetEnumerator();

            expectedEnumerator.Reset();
            actualEnumerator.Reset();
            expectedEnumerator.MoveNext();
            actualEnumerator.MoveNext();

            while (expectedEnumerator.Current != null)
            {
                if (!elementPredicate(expectedEnumerator.Current, actualEnumerator.Current))
                    throw new AssertFailedException("Collections are not equal!");

                expectedEnumerator.MoveNext();
                actualEnumerator.MoveNext();
            }            
        }
    }
}

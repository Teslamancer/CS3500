// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta 
//               (Clarified meaning of dependent and dependee.)
//               (Clarified names in solution/project structure.)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadsheetUtilities
{

    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        //Creates a Dictionary of Hashsets, where for each dependee key, there is a hashset of the dependents as the value
        private Dictionary<String, HashSet<String>> dependentDict;
        //Creates a Dictionary of Hashsets, where for each dependent key, there is a hashset of the dependees as the value
        private Dictionary<String, HashSet<String>> dependeeDict;
        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            dependentDict = new Dictionary<string, HashSet<string>>();
            dependeeDict = new Dictionary<string, HashSet<string>>();
        }


        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get;
            private set;
        }


        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {
            get { return getDependeeSetSize(s); }
        }

        /// <summary>
        /// Returns the number of Dependees for a given dependent
        /// </summary>
        /// <param name="s">Dependee to search</param>
        /// <returns>number of dependents</returns>
        private int getDependeeSetSize(string s)
        {
            if (dependeeDict.ContainsKey(s))
                return dependeeDict[s].Count;
            else
                return 0;

        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            if (dependentDict.ContainsKey(s))
                return dependentDict[s].Count != 0;
            else
                return false;
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            return this[s] != 0;
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (dependentDict.ContainsKey(s))
                return dependentDict[s];
            else
                return new HashSet<String>();
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (dependeeDict.ContainsKey(s))
                return dependeeDict[s];
            else
                return new HashSet<String>();
        }


        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>        /// 
        public void AddDependency(string s, string t)
        {
            if (dependentDict.ContainsKey(s))
            {
                dependentDict[s].Add(t);
                if (dependeeDict.ContainsKey(t))
                {
                    dependeeDict[t].Add(s);
                }
                else
                {
                    dependeeDict.Add(t, new HashSet<string>());
                    dependeeDict[t].Add(s);
                }
            }
            else
            {
                dependentDict.Add(s, new HashSet<string>());
                dependentDict[s].Add(t);
                if (dependeeDict.ContainsKey(t))
                {
                    dependeeDict[t].Add(s);
                }
                else
                {
                    dependeeDict.Add(t, new HashSet<string>());
                    dependeeDict[t].Add(s);
                }
            }
            Size++;
        }


        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            bool didRemove = false;
            if (dependentDict.ContainsKey(s) && dependentDict[s].Contains(t))
            {
                dependentDict[s].Remove(t);
                if (dependentDict[s].Count == 0)
                    dependentDict.Remove(s);
                didRemove = true;
            }
            
            if (dependeeDict.ContainsKey(t) && dependeeDict[t].Contains(s))
            {
                dependeeDict[t].Remove(s);
                if (dependeeDict[t].Count == 0)
                    dependeeDict.Remove(t);
                didRemove = true;              
            }
            if (didRemove)
                Size--;
            
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {            
            if (dependentDict.ContainsKey(s))
            {
                foreach(String d in dependentDict[s])
                {
                    dependeeDict[d].Remove(s);
                    if (dependeeDict[d].Count == 0)
                        dependeeDict.Remove(d);
                }
                Size -= dependentDict[s].Count;
                dependentDict[s] = new HashSet<String>(newDependents);
                
            }
            else
            {
                dependentDict.Add(s, new HashSet<String>(newDependents));
            }
            foreach(String t in dependentDict[s])
            {
                if (dependeeDict.ContainsKey(t))
                    dependeeDict[t].Add(s);
                else
                {
                    dependeeDict.Add(t, new HashSet<string>());
                    dependeeDict[t].Add(s);
                }
            }
            Size += dependentDict[s].Count;
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            if (dependeeDict.ContainsKey(s))
            {
                foreach (String d in dependeeDict[s])
                {
                    dependentDict[d].Remove(s);
                    if (dependentDict[d].Count == 0)
                        dependentDict.Remove(d);
                }
                Size -= dependeeDict[s].Count;
                dependeeDict[s] = new HashSet<String>(newDependees);
            }
            else
            {
                dependeeDict.Add(s, new HashSet<String>(newDependees));
            }
            foreach (String t in dependeeDict[s])
            {
                if (dependentDict.ContainsKey(t))
                    dependentDict[t].Add(s);
                else
                {
                    dependentDict.Add(t, new HashSet<string>());
                    dependentDict[t].Add(s);
                }
            }
            Size += dependeeDict[s].Count;
        }

    }

}

/* Adapted from https://github.com/Bl4ckb0ne/delaunay-triangulation

Copyright (c) 2015-2019 Simon Zeni (simonzeni@gmail.com)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.*/

using System;
using System.Collections.Generic;
using System.Numerics;

namespace AllegianceForms.Engine.Generation
{
    public class Delaunay2D<T> 
    {
        public List<Vertex<T>> Vertices { get; private set; }
        public List<Edge<T>> Edges { get; private set; }
        public List<Triangle<T>> Triangles { get; private set; }

        Delaunay2D() {
            Edges = new List<Edge<T>>();
            Triangles = new List<Triangle<T>>();
        }

        public static Delaunay2D<T> Triangulate(List<Vertex<T>> vertices)
        {
            var delaunay = new Delaunay2D<T>
            {
                Vertices = new List<Vertex<T>>(vertices)
            };
            delaunay.Triangulate();

            return delaunay;
        }

        void Triangulate() {
            var minX = Vertices[0].Position.X;
            var minY = Vertices[0].Position.Y;
            var maxX = minX;
            var maxY = minY;

            foreach (var vertex in Vertices) {
                if (vertex.Position.X < minX) minX = vertex.Position.X;
                if (vertex.Position.X > maxX) maxX = vertex.Position.X;
                if (vertex.Position.Y < minY) minY = vertex.Position.Y;
                if (vertex.Position.Y > maxY) maxY = vertex.Position.Y;
            }

            var dx = maxX - minX;
            var dy = maxY - minY;
            var deltaMax = Math.Max(dx, dy) * 2;

            var p1 = new Vertex<T>(new Vector2(minX - 1         , minY - 1          ));
            var p2 = new Vertex<T>(new Vector2(minX - 1         , maxY + deltaMax   ));
            var p3 = new Vertex<T>(new Vector2(maxX + deltaMax  , minY - 1          ));

            Triangles.Add(new Triangle<T>(p1, p2, p3));

            foreach (var vertex in Vertices) {
                var polygon = new List<Edge<T>>();

                foreach (var t in Triangles) {
                    if (t.CircumCircleContains(vertex.Position)) {
                        t.IsBad = true;
                        polygon.Add(new Edge<T>(t.A, t.B));
                        polygon.Add(new Edge<T>(t.B, t.C));
                        polygon.Add(new Edge<T>(t.C, t.A));
                    }
                }
                Triangles.RemoveAll(_ => _.IsBad);

                for (var i = 0; i < polygon.Count; i++) {
                    for (var j = i + 1; j < polygon.Count; j++) {
                        if (polygon[i] == polygon[j]) {
                            polygon[i].IsBad = true;
                            polygon[j].IsBad = true;
                        }
                    }
                }
                polygon.RemoveAll(_ => _.IsBad);

                foreach (var edge in polygon) {
                    Triangles.Add(new Triangle<T>(edge.U, edge.V, vertex));
                }
            }

            Triangles.RemoveAll(_ => _.ContainsVertex(p1.Position) 
                                    || _.ContainsVertex(p2.Position) 
                                    || _.ContainsVertex(p3.Position));

            var edgeSet = new HashSet<Edge<T>>();

            foreach (var t in Triangles) {
                var ab = new Edge<T>(t.A, t.B);
                var bc = new Edge<T>(t.B, t.C);
                var ca = new Edge<T>(t.C, t.A);

                if (edgeSet.Add(ab)) {
                    Edges.Add(ab);
                }

                if (edgeSet.Add(bc)) {
                    Edges.Add(bc);
                }

                if (edgeSet.Add(ca)) {
                    Edges.Add(ca);
                }
            }
        }
    }
    public class Vertex<T> : IEquatable<Vertex<T>>
    {
        public Vertex(Vector2 v, T i)
        {
            Position = v;
            Item = i;
        }

        public Vertex(Vector2 v) : this(v, default)
        { }

        public Vector2 Position { get; internal set; }
        public T Item { get; internal set; }

        public override bool Equals(object other)
        {
            return other is Vertex<T> ? Equals((Vertex<T>)other) : false;
        }

        public bool Equals(Vertex<T> other)
        {
            return Position == other.Position;
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }
    }

    public class Edge<T>
    {
        public float Distance { get; private set; }
        public Vertex<T> U { get; set; }
        public Vertex<T> V { get; set; }
        public bool IsBad { get; set; }
        public bool Selected { get; set; }

        public Edge(Vertex<T> u, Vertex<T> v)
        {
            U = u;
            V = v;
            Distance = Vector2.Distance(u.Position, v.Position);
        }

        public static bool operator ==(Edge<T> left, Edge<T> right)
        {
            return (left.U == right.U && left.V == right.V)
                || (left.U == right.V && left.V == right.U);
        }

        public static bool operator !=(Edge<T> left, Edge<T> right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj is Edge<T> e)
            {
                return this == e;
            }

            return false;
        }

        public bool Equals(Edge<T> e)
        {
            return this == e;
        }

        public override int GetHashCode()
        {
            return U.GetHashCode() ^ V.GetHashCode();
        }
    }
    public class Triangle<T> : IEquatable<Triangle<T>>
    {
        public Vertex<T> A { get; set; }
        public Vertex<T> B { get; set; }
        public Vertex<T> C { get; set; }
        public bool IsBad { get; set; }

        public Triangle(Vertex<T> a, Vertex<T> b, Vertex<T> c)
        {
            A = a;
            B = b;
            C = c;
        }

        public bool ContainsVertex(Vector2 v)
        {
            return Vector2.Distance(v, A.Position) < 0.01f
                || Vector2.Distance(v, B.Position) < 0.01f
                || Vector2.Distance(v, C.Position) < 0.01f;
        }

        public static bool operator ==(Triangle<T> left, Triangle<T> right)
        {
            return (left.A == right.A || left.A == right.B || left.A == right.C)
                && (left.B == right.A || left.B == right.B || left.B == right.C)
                && (left.C == right.A || left.C == right.B || left.C == right.C);
        }

        public static bool operator !=(Triangle<T> left, Triangle<T> right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj is Triangle<T> t)
            {
                return this == t;
            }

            return false;
        }

        public bool Equals(Triangle<T> t)
        {
            return this == t;
        }

        public override int GetHashCode()
        {
            return A.GetHashCode() ^ B.GetHashCode() ^ C.GetHashCode();
        }

        public bool CircumCircleContains(Vector2 v)
        {
            var a = A.Position;
            var b = B.Position;
            var c = C.Position;

            var ab = a.LengthSquared();
            var cd = b.LengthSquared();
            var ef = c.LengthSquared();

            var circumX = (ab * (c.Y - b.Y) + cd * (a.Y - c.Y) + ef * (b.Y - a.Y)) / (a.X * (c.Y - b.Y) + b.X * (a.Y - c.Y) + c.X * (b.Y - a.Y));
            var circumY = (ab * (c.X - b.X) + cd * (a.X - c.X) + ef * (b.X - a.X)) / (a.Y * (c.X - b.X) + b.Y * (a.X - c.X) + c.Y * (b.X - a.X));

            var circum = new Vector2(circumX / 2, circumY / 2);
            var circumRadius = (a - circum).LengthSquared();
            float dist = (v - circum).LengthSquared();
            return dist <= circumRadius;
        }
    }

}
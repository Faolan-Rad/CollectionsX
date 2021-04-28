﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
using BaseX;
using FrooxEngine.UIX;
using CollectionsX.Objs;
using CollectionsX.Delegates;

namespace CollectionsX.Objs
{
    public class RefArrayX<T> : SyncObject, ArrayX<T> where T : class, IWorldElement
    {
        private readonly SyncRefList<CollectionsItemRef<T>> Array;

        private readonly SyncBag<CollectionsItemRef<T>> Save;

        public event ArrayXDataChange<T> DataWritten;

        public event ArrayXDataChange<T> DataInsert;

        public event ArrayXLengthChange<T> DataShortened;

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        public string ToString()
        {
            return "a";
        }

        public ICollectionsObj<T> GetObj(int index)
        {
            return Array[index];
        }

        private CollectionsItemRef<T> MakeObj(T val)
        {
            CollectionsItemRef<T> tempObj = Save.Add();
            tempObj.Set(val);
            return tempObj;
        }
        public int Count { get { return Array.Count; } }

        public T this[int index]
        {
            get
            {
                return Array[index].Get();
            }
            set
            {
                Write(value, index);
            }
        }

        public void Clear()
        {
            base.World.RunSynchronously(delegate
            {
                int count = Count;
                for (int i = 0; i < count; i++)
                {
                    Array[0].Dispose();
                    Array.RemoveAt(0);
                }
                DataShortened(this, 0, count);
            });
        }

        public void Append(T value = default(T))
        {
            base.World.RunSynchronously(delegate
            {
                Array.Add(MakeObj(value));
                DataWritten(this, Count - 1, 1);

            });
        }

        public int Add(T value = default(T))
        {
            Append(value);
            return Count - 1;
        }
        public void RemoveAt(int index)
        {
            base.World.RunSynchronously(delegate
            {
                Save.Remove(Array[index]);
                Array.RemoveAt(index);
                DataShortened(this, index, 1);

            });
        }

        public void Remove(int index, int count)
        {
            base.World.RunSynchronously(delegate
            {
                for (int i = 0; i < count; i++)
                {
                    Array[index + i].Dispose();
                    Array.RemoveAt(i);
                }

                DataShortened(this, index, count);
            });
        }


        public void Write(T value, int index)
        {
            base.World.RunSynchronously(delegate
            {
                Array[index].Value.Target = value;
                DataWritten(this, index, 1);
            });
        }

        public void Insert(T value, int index)
        {
            base.World.RunSynchronously(delegate
            {
                Array.Insert(index, MakeObj(value));
                DataInsert(this, index, 1);
            });
        }

        public int IndexOf(T value = default(T))
        {
            for (int i = 0; i < Count; i++)
            {
                if (Array[i].SameValue(value))
                {
                    return i;
                }
            }
            return -1;
        }
        public override bool Equals(object other)
        {
            if (other is ArrayX<T>)
            {
                return this.Equals((ArrayX<T>)other);
            }
            return false;
        }

        public void Copy(ArrayX<T> source)
        {
            base.World.RunSynchronously(delegate
            {
                Remove(-1, Count);
                foreach (T a in source)
                {
                    Append(a);
                }
            });
        }

        public bool Equals(ArrayX<T> other)
        {
            bool same = other.Count == Count;
            if (same)
            {
                for (int index = 0; index < this.Count; index++)
                {
                    same = EqualityComparer<T>.Default.Equals(this[index], other[index]) && same;
                }
            }
            return same;
        }

        public void BuildInspectorUI(UIBuilder ui)
        {
            ui.PushStyle();
            ui.Style.MinHeight = -1f;
            ui.VerticalLayout(4f);
            ui.Style.MinHeight = 24f;
            ui.Style.MinHeight = -1f;
            ui.VerticalLayout(4f);
            ArrayXEditor<T> obj = ui.Root.AttachComponent<ArrayXEditor<T>>();
            ui.NestOut();
            ui.Style.MinHeight = 24f;
            LocaleString text;
            text = "Add";
            Button addButton;
            addButton = ui.Button(in text);
            ui.NestOut();
            obj.Setup(this, addButton);
            ui.PopStyle();
        }
    }


}

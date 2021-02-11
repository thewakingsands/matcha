// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Cafe.Matcha.Utils;

    internal class FateNode : FateTreeNode
    {
        public FateNode(int id, Models.ItemName name)
        {
            Id = id;
            Name = name;
        }

        public override string LocalName
        {
            get
            {
                return Name.ToString();
            }
        }

        public int Id { get; set; }

        public override bool IsChecked { get; set; } = false;

        public Models.ItemName Name;
    }

    internal class FateVersionTree : FateTreeNodeWithChildren
    {
        public FateVersionTree(int patchId)
        {
            PatchId = patchId;
            if (!Data.Instance.Patches.TryGetValue(patchId, out Patch))
            {
                throw new Exception(string.Format("Failed to find patch #{0}", patchId));
            }

            Children = new ObservableCollection<FateTreeNode>();
        }

        public override string LocalName
        {
            get
            {
                return Patch.ToString();
            }
        }

        public int PatchId;
        public Models.ItemName Patch;
    }

    internal class FateTerritoryTree : FateTreeNodeWithChildren
    {
        private static Models.ItemName multipleTerritories = new Models.ItemName()
        {
            Chinese = "多个地区",
            /*
            English = "Multiple Territories",
            Japanese = "複数の地域",
            German = "Mehrere Gebiete",
            Franch = "Territoires multiples"
            */
        };

        public FateTerritoryTree(int territoryId)
        {
            if (territoryId == 0)
            {
                Territory = multipleTerritories;
            }
            else if (!Data.Instance.Territories.TryGetValue(territoryId, out Territory))
            {
                throw new Exception(string.Format("Failed to find territory #{0}", territoryId));
            }

            TerritoryId = territoryId;
            Children = new ObservableCollection<FateTreeNode>();
        }

        public override string LocalName
        {
            get
            {
                return Territory.ToString();
            }
        }

        public int TerritoryId;
        public Models.ItemName Territory;
    }

    public abstract class FateTreeNodeWithChildren : FateTreeNode
    {
        public override bool IsChecked
        {
            get
            {
                return Children.All(item => item.IsChecked);
            }
            set
            {
                foreach (var item in Children)
                {
                    item.IsChecked = value;
                }
            }
        }

        public IEnumerable<FateTreeNode> Leaves
        {
            get
            {
                return Children.SelectMany<FateTreeNode, FateTreeNode>(item =>
                {
                    if (item is FateTreeNodeWithChildren)
                    {
                        return ((FateTreeNodeWithChildren)item).Children;
                    }
                    else
                    {
                        return new[] { item };
                    }
                });
            }
        }

        public ObservableCollection<FateTreeNode> Children { get; set; }

        public static ObservableCollection<FateTreeNodeWithChildren> Create(Dictionary<int, Models.FateData> fates)
        {
            var result = new ObservableCollection<FateTreeNodeWithChildren>()
            {
                new FateTerritoryTree(0)
            };

            if (fates == null)
            {
                return result;
            }

            foreach (var pair in fates)
            {
                if (pair.Value.Location == 0)
                {
                    ((FateTerritoryTree)result[0]).Children.Add(new FateNode(pair.Key, pair.Value.Name));
                }
                else
                {
                    FateVersionTree versionTree = (FateVersionTree)result.FirstOrDefault(tree =>
                        tree is FateVersionTree && pair.Value.Patch == ((FateVersionTree)tree).PatchId);

                    if (versionTree == null)
                    {
                        versionTree = new FateVersionTree(pair.Value.Patch);
                        result.Add(versionTree);
                    }

                    FateTerritoryTree territoryTree = (FateTerritoryTree)versionTree.Children.FirstOrDefault(tree =>
                        tree is FateTerritoryTree && pair.Value.Location == ((FateTerritoryTree)tree).TerritoryId);

                    if (territoryTree == null)
                    {
                        territoryTree = new FateTerritoryTree(pair.Value.Location);
                        versionTree.Children.Add(territoryTree);
                    }

                    territoryTree.Children.Add(new FateNode(pair.Key, pair.Value.Name));
                }
            }

            return result;
        }
    }

    public abstract class FateTreeNode : BindingTarget
    {
        public abstract string LocalName { get; }

        public abstract bool IsChecked { get; set; }
    }

    public class MainViewModel : BindingTarget
    {
        public MainViewModel()
        {
            Config.Instance.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "UUID")
                {
                    EmitPropertyChanged("DataReport");
                }
            };
        }

        public string DataReport
        {
            get
            {
                var uuid = Config.Instance.UUID;
                if (uuid == null)
                {
                    return "未设置";
                }

                return uuid == "no" ? "禁用" : "启用";
            }
        }

        public string Log { get; set; } = "";
        public Models.Template SelectedTemplate { get; set; } = null;
        public ObservableCollection<FateTreeNodeWithChildren> Fates { get; set; } = FateTreeNodeWithChildren.Create(null);
        public Models.ConfigWebhook SelectedWebhook { get; set; } = null;
        public bool EnableWebhook
        {
            get
            {
                return SelectedWebhook != null;
            }
        }
    }
}

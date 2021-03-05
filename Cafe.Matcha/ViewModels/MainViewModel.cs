// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using Cafe.Matcha.Utils;
    using FFXIV_ACT_Plugin.Common;

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
            Chinese = "未知地区",
            English = "Unknown Area",
            Japanese = "不明なエリア",
            German = "Unbekannter Bereich",
            French = "Zone inconnue"
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

        public static ListBindingTarget<FateTreeNodeWithChildren> Create(Dictionary<int, Models.FateData> fates)
        {
            var result = new ListBindingTarget<FateTreeNodeWithChildren>()
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

    public struct L12nRegion
    {
        public Constant.Region ID { get; set; }
        public Models.ItemName Name { get; set; }
    }

    public struct L12nLanguage
    {
        public Language ID { get; set; }
        public string Name { get; set; }
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

                if (e.PropertyName == "Language")
                {
                    var ne = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                    Regions.EmitCollectionChanged(ne);
                    Fates.EmitCollectionChanged(ne);
                    Templates.EmitCollectionChanged(ne);
                }
            };

            Data.Instance.DataLoaded += (sender, e) =>
            {
                Templates = new ListBindingTarget<Models.Template>(Data.Instance.Templates);
            };
        }

        public ListBindingTarget<L12nLanguage> Languages { get; } = new ListBindingTarget<L12nLanguage>
        {
            new L12nLanguage
            {
                ID = Language.English,
                Name = "English"
            },
            new L12nLanguage
            {
                ID = Language.French,
                Name = "Français"
            },
            new L12nLanguage
            {
                ID = Language.German,
                Name = "Deutsche"
            },
            new L12nLanguage
            {
                ID = Language.Japanese,
                Name = "日本語"
            },
            new L12nLanguage
            {
                ID = Language.Chinese,
                Name = "中文"
            },
        };

        public ListBindingTarget<L12nRegion> Regions { get; } = new ListBindingTarget<L12nRegion>
        {
            new L12nRegion
            {
                ID = Constant.Region.Global,
                Name = new Models.ItemName
                {
                    English = "Global",
                    Chinese = "国际服"
                }
            },
            new L12nRegion
            {
                ID = Constant.Region.China,
                Name = new Models.ItemName
                {
                    English = "China",
                    Chinese = "国服"
                }
            },
        };

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
        public ListBindingTarget<Models.Template> Templates { get; set; } = Data.Instance.Templates != null ? new ListBindingTarget<Models.Template>(Data.Instance.Templates) : null;
        public Models.Template SelectedTemplate { get; set; } = null;
        public ListBindingTarget<FateTreeNodeWithChildren> Fates { get; set; } = FateTreeNodeWithChildren.Create(null);
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

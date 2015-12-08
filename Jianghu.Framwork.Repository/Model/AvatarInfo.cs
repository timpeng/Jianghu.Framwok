using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;

namespace Jianghu.Framwork.Repository.Model
{
    /// <summary>
    /// 游戏角色表
    /// </summary>
    public class AvatarInfo
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string uID { get; set; }
        /// <summary>
        /// 角色名
        /// </summary>
        public string aName { get; set; }
        /// <summary>
        /// 在线时间
        /// </summary>
        public int aPlayTime { get; set; }
        /// <summary>
        /// 派别 0 1 2，正邪魔
        /// </summary>
        public int aTribe { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public int aLevel { get; set; }
        /// <summary>
        /// 经验
        /// </summary>
        public long aGeneralExperience { get; set; }
        /// <summary>
        /// 资质
        /// </summary>
        public int aSkillPoint { get; set; }
        /// <summary>
        /// 属性点
        /// </summary>
        public int aStateBonusPoint { get; set; }
        /// <summary>
        /// 体力
        /// </summary>
        public int aVitality { get; set; }
        /// <summary>
        /// 力量
        /// </summary>
        public int aStrength { get; set; }
        /// <summary>
        /// 内力
        /// </summary>
        public int aKi { get; set; }
        /// <summary>
        /// 敏捷
        /// </summary>
        public int aWisdom { get; set; }
        /// <summary>
        /// 雪丹数量
        /// </summary>
        public int aEatLifePotion { get; set; }
        /// <summary>
        /// 蓝丹数量
        /// </summary>
        public int aEatManaPotion { get; set; }
        /// <summary>
        /// 背包物品
        /// </summary>
        public string aEquip { get; set; }
        /// <summary>
        /// 背包物品的数值
        /// </summary>
        public string aEquipValue { get; set; }
        /// <summary>
        /// 背包游戏币
        /// </summary>
        public int aMoneyHONG { get; set; }
        /// <summary>
        /// 仓库游戏币
        /// </summary>
        public int aStoreMoneyHONG { get; set; }
        /// <summary>
        /// 仓库物品1
        /// </summary>
        public string aStoreItem { get; set; }
        /// <summary>
        /// 仓库物品1的数值
        /// </summary>
        public string aStoreItemValue { get; set; }
        /// <summary>
        /// 仓库物品2
        /// </summary>
        public string aStoreItem2 { get; set; }
        /// <summary>
        /// 仓库物品2的数值
        /// </summary>
        public string aStoreItemValue2 { get; set; }
        /// <summary>
        /// 仓库物品3
        /// </summary>
        public string aStoreItem3 { get; set; }
        /// <summary>
        /// 仓库物品3的数值
        /// </summary>
        public string aStoreItemValue3 { get; set; }

        /// <summary>
        /// 技能的值
        /// </summary>
        public int aSkill { get; set; }
        /// <summary>
        /// 力量丹
        /// </summary>
        public int aEatStrengthPotion { get; set; }
        /// <summary>
        /// 敏捷丹
        /// </summary>
        public int aEatDexterityPotion { get; set; }
        /// <summary>
        /// 人物杀人贡献值
        /// </summary>
        public int aKillOtherTribe { get; set; }
    }
}

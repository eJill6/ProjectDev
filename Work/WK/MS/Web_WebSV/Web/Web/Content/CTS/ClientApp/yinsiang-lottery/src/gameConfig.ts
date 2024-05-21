const correspondContext: { [key: string]: string } = {
  "豹子": "BaoZi",
  "单骰": "DanTou",
  "对子": "DuiZi",
  "两面": "LiangMian",
  "总和": "ZongHe",
  "冠军与两面": "KuaiXuanGuanJunYuLiangMian",
  "冠亚和": "KuaiXuanGuanYaHe",
  "冠军特殊": "KuaiXuanGuanJunTeShu",
  "冠军": "KuaiXuanGuanJun",
  "龙虎球1VS球5": "KuaiXuanLongHuQiu1VsQiu5",
  "特码两面": "KuaiXuanTaMaLiangMian",
  "特码": "KuaiXuanTaMa",
  "特码生肖": "KuaiXuanTaMaShengXiao",
  "特码色波": "KuaiXuanTaMaSeBo"
};

const basePlayTypeId: number[] = [17, 19, 34, 42];

const basePlayTypeRadioId: number[] = [26, 62, 75, 173];

const shengXiaoContext: string = "特码生肖";

export { correspondContext, basePlayTypeId, basePlayTypeRadioId, shengXiaoContext };

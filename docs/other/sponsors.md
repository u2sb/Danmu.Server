---
title: 捐赠
---

# 捐赠

::: tip 联系
如果您对弹幕服务感兴趣，想要赞助作者继续开发，欢迎联系作者 xxwhite@foxmail.com
:::

::: tip 赞助商
目前还没有赞助商
:::

::: tip 捐赠
如果我的项目对您有帮助，您也可以通过下面的链接赞助我（备注弹幕服务器捐赠），较大金额的赞助会永久列在下面的赞助列表中，本项目所有所得赞助，均会用于本项目的持续开发和公共服务的搭建。
:::

<center style="display:flex; justify-content:space-around;">
<a href="https://afdian.net/@monologuechi" target="_blank"><img class="sponsors-img" src="@img/other/afdian-MonoLogueChi.png"></a>
<img src="@img/other/alipay.png" class="sponsors-img">
<img src="@img/other/wechatpay.png" class="sponsors-img">
</center>

截至 {{date}}，本项目共收到捐款 {{receivables}} 元
截至 {{date}}，本项目共支出 {{pay}} 元

> 感谢 `*永虎` 捐赠的 50 元。
> 感谢 `天空铃音` 捐赠的 13.32 元。

---

## 感谢

[![](@img/other/ReSharper.png =200x200)](https://www.jetbrains.com/resharper/)

**使用 ReSharper 开发**

<script>
const date = new Date();
const year = date.getFullYear();
const month = date.getMonth() + 1;
var start_time = "2020-1";
var arr = start_time.split('-');
var start_year = arr[0];
var start_month = arr[1];
var count = ( year-start_year ) * 12 + month - start_month +1;

const pay = 344 + count * 24;
const otherReceivables = 5.36 + 5;
const receivables = 50 + 13.32 + otherReceivables;

export default {
  data() {
    return {
      date: `${year}年${month}月`,
      pay: pay,
      receivables: receivables
    }
  }
}
</script>

<ClientOnly>
  <Vssue title="捐赠-Other | 弹幕服务器文档" />
</ClientOnly>

<style lang="stylus" scoped>
.sponsors-img {
  height: 400px;
  @media(max-width: 1600px) {
    height: 350px;
  }
  @media(max-width: 1450px) {
    height: 300px;
  }
}
</style>

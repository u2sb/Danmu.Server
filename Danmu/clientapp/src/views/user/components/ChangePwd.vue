<template>
  <el-form
    ref="form"
    :model="form"
    :rules="rules"
    size="small"
    label-position="right"
    label-width="100px"
    status-icon
  >
    <el-form-item label="旧密码：" prop="oldP">
      <el-input v-model="form.oldP" maxlength="100" type="password" />
    </el-form-item>
    <el-form-item label="新密码：" prop="newP">
      <el-input v-model="form.newP" maxlength="100" type="password" />
    </el-form-item>
    <el-form-item label="确认密码：" prop="confirmP">
      <el-input v-model="form.confirmP" type="password" />
    </el-form-item>
    <el-form-item>
      <el-button :loading="loading" type="primary" size="small" @click="submit">确认修改</el-button>
    </el-form-item>
  </el-form>
</template>

<script>
import { mapState } from "vuex";
import { changePwd } from "@/api/admin/account";
import { elAlert, elSuccess } from "@/utils/message";
import md5 from "js-md5";

export default {
  data() {
    return {
      loading: false,
      form: {
        oldP: null,
        newP: null,
        confirmP: null
      },
      rules: {
        oldP: [{ required: true, message: "请输入原密码", trigger: "change" }],
        newP: [
          { required: true, message: "请输入新密码", trigger: "change" },
          {
            validator: (r, v, c) =>
              v !== this.form.oldP ? c() : c("新密码不得与旧密码相同"),
            trigger: "change"
          },
          { min: 6, message: "密码长度不能低于6位", trigger: "change" }
        ],
        confirmP: [
          {
            validator: (r, v, c) =>
              v === this.form.newP ? c() : c("两次密码不一致"),
            trigger: "change"
          }
        ]
      }
    };
  },
  computed: {
    ...mapState("user", {
      id: state => state.id
    })
  },
  methods: {
    clearForm() {
      this.form.oldP = null;
      this.form.newP = null;
      this.form.confirmP = null;
      this.$refs.form.resetFields();
    },
    submit() {
      if (this.loading) return;
      this.$refs.form.validate(v => {
        if (!v) return;
        this.loading = true;
        changePwd({
          uid: this.id,
          oldP: md5(this.form.oldP),
          newP: md5(this.form.newP)
        })
          .then(() =>
            elAlert("修改成功，请重新登录", () =>
              this.$store.dispatch("user/logout")
            )
          )
          .finally(() => {
            this.loading = false;
            this.clearForm();
          });
      });
    }
  }
};
</script>

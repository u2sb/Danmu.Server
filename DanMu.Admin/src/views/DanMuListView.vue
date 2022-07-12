<template>
  <a-table :columns="columns" :data-source="danmuList">
    <template #bodyCell="{ column, record, index }">
      <template v-if="column.key === 'cTime'">
        {{ new Date(parseInt(record.cTime)).toLocaleString() }}
      </template>
      <template v-else-if="column.key === 'action'">
        <span>
          <a v-on:click="deleteDanMu(record.id, index)">删除</a>
        </span>
      </template>
    </template>
  </a-table>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import api from "@/api/api.json";

export default defineComponent({
  setup() {},

  data() {
    return {
      columns: [
        {
          title: "Id",
          dataIndex: "id",
          key: "id",
        },
        {
          title: "Vid",
          dataIndex: "videoId",
          key: "videoId",
        },
        {
          title: "内容",
          dataIndex: "content",
          key: "content",
        },
        {
          title: "发送时间",
          dataIndex: "cTime",
          key: "cTime",
        },
        {
          title: "操作",
          dataIndex: "action",
          key: "action",
        },
      ],
      danmuList: [],
    };
  },

  methods: {
    deleteDanMu(id: string, i: number) {
      fetch(`${api.danmuDelete}/${id}`)
        .then((res) => res.json())
        .then((res) => {
          if (res.code === 0) {
            this.danmuList.splice(i, 1);
          }
        });
    },
  },

  mounted() {
    fetch(api.danmuList)
      .then((res) => res.json())
      .then((res) => {
        if (res.code === 0) {
          return res.data;
        } else if (res.code === 401) {
          this.$router.push("/login");
        }
      })
      .then((data) => {
        this.danmuList = data;
      });
  },
});
</script>

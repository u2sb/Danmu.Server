import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

const store = new Vuex.Store({
  state: {
    admin: {
      id: '',
      vid: '',
      vids: [''],
      showVidSelect: false
    }
  },
  mutations: {
    changeId(state, id) {
      state.admin.id = id
    },
    changeVid(state, vid) {
      state.admin.vid = vid
    },
    changeVids(state, vids) {
      state.admin.vids = vids
    },
    changeShowVidSelect(state, value) {
      state.admin.showVidSelect = value
    }
  }
})

export default store

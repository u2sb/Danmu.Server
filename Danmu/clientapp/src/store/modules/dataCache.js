/*
* 数据缓存
* */

const state = {
    //搜索过的vid
    searchedVids: []
}

const mutations = {
    setSearchedVids(state, vids) {
        state.searchedVids = vids || []
    }
}

const actions = {}


export default {
    namespaced: true,
    state,
    mutations,
    actions
}

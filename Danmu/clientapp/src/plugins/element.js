import Vue from 'vue'
import '../element-variables.scss'

import {
  Container,
  Header,
  Main,
  Aside,
  Footer,
  Col,
  Row,
  Button,
  ButtonGroup,
  Select,
  Form,
  FormItem,
  Input,
  Link,
  Menu,
  Submenu,
  MenuItem,
  MenuItemGroup,
  Tabs,
  TabPane,
  Table,
  TableColumn,
  DatePicker,
  TimeSelect,
  TimePicker,
  ColorPicker,
  Popover,
  Option,
  OptionGroup,
  Pagination,
  Dialog,
  Notification,
  Popconfirm,
  MessageBox,
  Message,
  Loading
} from 'element-ui'

Vue.use(Container)
Vue.use(Header)
Vue.use(Main)
Vue.use(Aside)
Vue.use(Footer)
Vue.use(Col)
Vue.use(Row)
Vue.use(Button)
Vue.use(ButtonGroup)
Vue.use(Select)
Vue.use(Form)
Vue.use(FormItem)
Vue.use(Input)
Vue.use(Link)
Vue.use(Menu)
Vue.use(Submenu)
Vue.use(MenuItem)
Vue.use(MenuItemGroup)
Vue.use(Tabs)
Vue.use(TabPane)
Vue.use(Table)
Vue.use(TableColumn)
Vue.use(DatePicker)
Vue.use(TimeSelect)
Vue.use(TimePicker)
Vue.use(ColorPicker)
Vue.use(Popover)
Vue.use(Option)
Vue.use(Pagination)
Vue.use(Dialog)
Vue.use(Popconfirm)
Vue.use(OptionGroup)

Vue.use(Loading.directive)

Vue.prototype.$loading = Loading.service
Vue.prototype.$notify = Notification
Vue.prototype.$msgbox = MessageBox
Vue.prototype.$alert = MessageBox.alert
Vue.prototype.$confirm = MessageBox.confirm
Vue.prototype.$prompt = MessageBox.prompt
Vue.prototype.$message = Message

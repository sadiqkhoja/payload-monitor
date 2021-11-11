<template>
  <div>    
    <Login v-if="loginRequired" @loginSuccessful="hideLogin()"></Login>
    <template v-if="!loginRequired">

      <select name="environment" id="environment" v-model="environment" @change="updateEnv()">
        <option value="DEV_r12">DEV R12</option>
        <option value="DEV_ocloud">DEV Oracle Cloud</option>
        <option value="QAS_r12">QAS R12</option>
        <option value="QAS_ocloud">QAS Oracle Cloud</option>
      </select>

      <div v-if="isLoading" class='loading'>loading...</div>
      <PayloadPopup :visible="popupVisibility" :data="payload" @popupClose="closePopup()"></PayloadPopup>
      <ListOfInterface @interfaceClicked="fetchRequests($event)"></ListOfInterface>
      <h1>{{heading}}</h1>
      <DataTable v-if="!isLoading"  :rows="rows" :columns="columns" @rowClicked="openPopup($event)"></DataTable>
    </template>
    

  </div>
</template>

<script>
// import HelloWorld from './components/HelloWorld.vue'
import ListOfInterface from './components/ListOfInterfaces.vue'
import DataTable from './components/DataTable.vue'
import PayloadPopup from './components/PayloadPopup.vue'
import Login from './components/Login.vue'
import config from './components/Config.vue'

export default {
  name: 'App',
  components: {
    ListOfInterface,
    DataTable,
    PayloadPopup,
    Login
  },
  data(){
    return {
      rows: [],
      columns: [],
      popupVisibility: '',
      payload:[],
      heading: '',
      isLoading: false,
      username: '',
      password: '',
      loginRequired: true,
      environment: '',
      config: []
    } 
  },  
  created(){
    console.log(config);

    this.config = config;

    var loginTime = localStorage.getItem('loginTime');    
    if( Number(loginTime) + (24*60*60*1000) < (new Date()).getTime()){
      this.loginRequired = true;
    }
    else{
      this.loginRequired = false;
    }

    var savedEnv = localStorage.getItem('environment');
    if(savedEnv){
      this.environment = savedEnv;
    }
    else{
      this.environment = 'QAS_ocloud'
    }

  },
  methods:{
    updateEnv(){
      localStorage.setItem('environment', this.environment);
    },
    hideLogin(){
      this.loginRequired = false;
    },
    closePopup(){
      this.popupVisibility = 'none';
    },
    openPopup(event){      
      this.fetchPayload(event);
    },
    fetchPayload(event){
      this.isLoading = true;
      let requestOptions = {
        method: 'GET',
        headers: this.getHeaders()
      };

      let baseUrl = `${process.env.VUE_APP_API_BASE_URL}/api/QueryAppInsight`;
      var query = `traces
| where message startswith "Ariba XML" or (message  startswith "Rest" and cloud_RoleName contains "integ")
| where operation_Id == "${event}"
| order by timestamp asc
| project message`;

        fetch(`${baseUrl}?environment=${this.environment.substr(0,3)}&code=${process.env.VUE_APP_APP_CODE}&query=${query}`, requestOptions)
        .then(response => response.text())
        .then(result => {
          let tables = JSON.parse(result).tables;
          this.payload = tables[0].rows;
          this.popupVisibility = 'block';
          this.isLoading = false;
        })
        .catch(error => {
          console.log('error', error);
          this.isLoading = false;
          alert(error);
        });

    },
    getHeaders(){
      return {
        'username':localStorage.getItem('username'),
        'password':localStorage.getItem('password')
      };
    },
    fetchRequests(event){
      this.isLoading = true;
      let requestOptions = {
        method: 'GET',
        headers: this.getHeaders()
      };

      let application = "";

      if(this.environment.endsWith("ocloud")){
        application = "aribaerpsoapgateway";
      }
      else{
        application = "aribasoapgateway";
      }

      let operationName = this.config[event].operationName;
      this.heading = this.config[event].heading;

      let baseUrl = `${process.env.VUE_APP_API_BASE_URL}/api/QueryAppInsight`;

      var query = `requests
| where cloud_RoleName contains "${application}"
| where operation_Name contains "${operationName}"
| order by timestamp desc 
| project operation_Id, timestamp, success`;

      fetch(`${baseUrl}?environment=${this.environment.substr(0,3)}&code=${process.env.VUE_APP_APP_CODE}&query=${query}`, requestOptions)
        .then(response => response.text())
        .then(result => {
          let tables = JSON.parse(result).tables;
          this.rows = tables[0].rows;
          this.columns = tables[0].columns;

          this.rows.forEach(r => {
            r[1] = (new Date(r[1])).toLocaleString();
          });
          this.isLoading = false;
        })
        .catch(error => {
          console.log('error', error);
          this.isLoading = false;
        });
    }
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
  ;
}
.loading{
    position: fixed;
    top: 0;
    font-size: 22px;
}
</style>

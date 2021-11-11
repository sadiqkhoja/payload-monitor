<template>
    <div>
        Username:
        <input type="text" name="username" id="username" v-model="username">
        <br/><br/>

        Password:
        <input type="password" name="password" id="password" v-model="password" @keyup="keyup($event)">
        <br/><br/>
        <button @click="setCredentials()">Login</button>
    </div>
</template>

<script>
export default {
    name: 'Login',
    data(){
        return {
            username:'',
            password:''
        };
    },
    methods:{
        keyup(event){
            if (event.keyCode === 13) {
                this.setCredentials();
            }
        },
        setCredentials(){
            let requestOptions = {
                method: 'POST',
                body: JSON.stringify({
                    username: this.username,
                    password: this.password
                })
            };

            let baseUrl = `${process.env.VUE_APP_API_BASE_URL}/api/ValidateCredentials?code=${process.env.VUE_APP_APP_CODE}`;
      

            fetch(`${baseUrl}`, requestOptions)
            .then(resp => {
                if(resp.ok){
                    localStorage.setItem('username', this.username);
                    localStorage.setItem('password', this.password);
                    localStorage.setItem('loginTime', (new Date()).getTime());
                    this.$emit('loginSuccessful');
                }  
                else{
                    resp.text().then(error => alert(error));
                }              
            })
            .catch(error => {
                alert(error);
            });


            
        }
    }
}
</script>

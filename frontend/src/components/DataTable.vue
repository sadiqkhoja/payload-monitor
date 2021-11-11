<template>
   <div>
        <!-- <div v-for="item in data" :key="item">
          {{item}}
        </div> -->

        <table v-if="rows.length > 0">
          <tr>
            <th v-for="col in columns" :key="col.name">
              {{col.name}}
            </th>
          </tr>  
          <tr v-for="row in rows" :key="row[0]">
            <td v-for="(cell,index) in row" :key="cell">
              <a v-if="index == 0" href="javascript:void(0)" @click="$emit('rowClicked', cell)">{{cell}}</a>
              <span v-else>{{cell}}</span>
            </td>
          </tr>        
        </table>
        <p v-else>No requests in last 4 hours</p>
   </div>
</template>

<script>
  export default {
    name: 'DataTable',
    props: {
      columns: Object,
      rows: Object
    }  
  };
</script>

<style scoped>
table{
  margin: 0 auto;
}
td{
  text-align: left;
  padding:10px;
  border-bottom: 1px solid black;
}
</style>
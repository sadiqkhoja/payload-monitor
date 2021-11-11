<template>
    <div class="table">
        <div class="row">
            <div class="cell color3" @click="$emit('interfaceClicked', 'taxCalculation')">Tax Calculation</div>
            <div class="cell color2" @click="$emit('interfaceClicked', 'coaValidationRequisition')">COA Validation - Requisition</div>
            <div class="cell color2" @click="$emit('interfaceClicked', 'coaValidationInvoice')">COA Validation - Invoice</div>
            <div class="cell color2" @click="$emit('interfaceClicked', 'coaValidationIR')">COA Validation - Invoice Reconciliation</div>
            <div class="cell color1" @click="$emit('interfaceClicked', 'invoice')">Invoice</div>
            <div class="cell color1" @click="$emit('interfaceClicked', 'advancePayment')">Advance Payment</div>
        </div>
    </div>    
</template>

<script>
export default {
  name: 'ListOfInterface',
  props: {
  }
};

</script>

<style scoped>
.table {
    display:table;
    border-collapse:separate;
    border-spacing:20px;
    margin:0 auto;
}
.row {
    display:table-row;
}
.cell {
    display:table-cell;
    padding:5px;
    border:1px solid black;
    height: 100px;
    width: 200px;
    font-size: 18px;
    vertical-align: middle;
    color:white;
    cursor: pointer;
}

.color1{
    background: #6b5b95;
}
.color2{
    background: #82b74b;
}
.color3{
    background: #d64161;
}
.color4{
    background: #ff7b25;
}
</style>
<script>
import {INITIAL_EVENTS} from "./utils";
import ChinhSua from "./chinhsua.vue";

export default {

  data() {
    return {
      data: INITIAL_EVENTS,
    };
  },
  components: {ChinhSua},
  methods: {},
};
</script>

<template>

</template>
<style>
.modal-title {
  color: #fff;
}

.bg-primary-dark {
  background: linear-gradient(135deg, #06548e, #ffffff);
  box-shadow: 0px 3px 0px #06548e;
}

.cl-warning {
  color: rgb(253, 216, 123);
}
</style>

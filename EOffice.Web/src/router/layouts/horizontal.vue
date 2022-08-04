<script>
import { layoutComputed } from "@/state/helpers";

import HorizontalTopbar from "@/components/horizontal-topbar";
import HorizontalNav from "@/components/horizontal-nav";
import Footer from "@/components/footer";
import RightBar from "@/components/right-bar";

/**
 * Horizontal-layout component
 */
export default {
  components: { HorizontalTopbar, HorizontalNav, RightBar, Footer },
  computed: {
    ...layoutComputed
  },
  created: () => {
    document.body.setAttribute("data-layout", "horizontal");
    document.body.setAttribute("data-topbar", "dark");
    document.body.removeAttribute('data-sidebar','dark');
  },
  methods: {
    toggleRightSidebar() {
      document.body.classList.toggle("right-bar-enabled");
    },
    hideRightSidebar() {
      document.body.classList.remove("right-bar-enabled");
    }
  }
};
</script>

<template>
  <div id="layout-wrapper">
    <HorizontalTopbar :width="layoutWidth" :type="topbar" />
    <HorizontalNav />

    <div class="main-content">
      <div class="page-content">
        <div class="container-fluid">
          <slot />
        </div>
      </div>
      <Footer />
    </div>
    <RightBar />
  </div>
</template>
<style>
@media only screen and (max-width: 768px){
  .navbar-brand-box {
    width: auto;
  }
  .navbar-brand-box .logo-light,
  .navbar-brand-box .logo-dark{
    display: block;
    width: 0px !important;
    height:  0px !important;
  }
  .navbar-brand-box img {
    max-width: 50px !important;
    height: auto;
    position: absolute;
    top: 10px;
  }
}

@media only screen and (max-width: 768px) {
  .navbar-brand-box {
    width: 80px;
    margin: auto !important;
  }
}
</style>
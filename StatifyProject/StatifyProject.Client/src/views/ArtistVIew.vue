<template>
  <div class="songView">
    <div class="center-title">
      <h1 class="title"><span style="color: #1db954">Top</span> Artists</h1>
    </div>
    <section id="profile" class="profile-section">
      <ul class="profile-list">
        <li
          v-for="(artist, index) in topArtists"
          :key="artist.name"
          @click="navigateToArtist(artist)"
        >
          <div class="card">
            <div class="card-content">
              <img
                :src="artist.images[0].url"
                :alt="artist.name"
                class="card-image"
              />
              <div class="card-text">
                <h3>{{ index + 1 + ". " + artist.name }}</h3>
                <h5 class="genre">{{ artist.genres[0] }}</h5>
              </div>
            </div>
          </div>
        </li>
      </ul>
    </section>
  </div>
</template>

<style scoped>
.center-title {
  text-align: center;
}
.title {
  color: white;
}
.songView .container {
  max-width: 900px;
  margin: 0 auto;
}

.profile-list {
  list-style-type: none;
  padding: 0;
  margin-top: 20px;
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
}

.card {
  width: 420px;
  border: 1px solid #1db954;
  margin: 5px;
  border-radius: 8px;
  overflow: hidden;
  background-color: #1db9542a;
  color: #ffffff;
  transition: transform 0.5s ease;
  display: flex;
}

.card:hover {
  transform: scale(1.05);
}

.card-image {
  width: 100px;
  height: 100px;
  object-fit: cover;
  margin-right: 16px;
}

.card-content {
  display: flex;
  align-items: center;
}

.card-text {
  display: flex;
  flex-direction: column;
}

.card-content h3,
.card-content h5 {
  margin: 0;
}

.genre {
  color: darkgray;
}
</style>

<script>
export default {
  data() {
    return {
      topArtists: [],
    };
  },
  mounted() {
    this.fetchTopArtist();
  },
  methods: {
    async fetchTopArtist() {
      const access_token = localStorage.getItem("access_token");
      const result = await fetch(
        "https://api.spotify.com/v1/me/top/artists?time_range=short_term&limit=20",
        {
          headers: {
            Authorization: `Bearer ${access_token}`,
          },
        }
      );
      const topArtists = await result.json();
      this.topArtists = topArtists.items;
      return topArtists;
    },

    navigateToArtist(artist) {
      const artistLink = this.getArtistLink(artist);
      if (artistLink) {
        const linkEl = document.createElement("a");
        linkEl.href = artistLink;
        linkEl.target = "_blank";
        linkEl.click();
      }
    },
    getArtistLink(artist) {
      return artist.external_urls.spotify;
    },
  },
};
</script>

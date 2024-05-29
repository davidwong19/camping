<template>
  <div class="camping-spot-profile">
    <div class="card">
      <div class="card-body">
        <div class="image-section">
          <img :src="`data:image/jpeg;base64,${campingSpot.images[0]}`" alt="Camping Spot Image" />
        </div>
        
        <div class="details-section">
          <h1><strong>{{ campingSpot.title }}</strong></h1>
          <p><strong>Country:</strong> {{ campingSpot.country }}</p>
          <p><strong>Location:</strong> {{ campingSpot.location }}</p>
          <p><strong>Theme:</strong> {{ campingSpot.theme }}</p>
          <p><strong>Number of Campers:</strong> {{ campingSpot.howmanycampers }}</p>
          <p><strong>On-site Accommodation:</strong> {{ campingSpot.onsiteaccommodation }}</p>
          <p><strong>Available:</strong> {{ campingSpot.isAvailable ? 'Yes' : 'No' }}</p>
          <p><strong>Begin Date:</strong> {{ campingSpot.beginDate }}</p>
          <p><strong>End Date:</strong> {{ campingSpot.endDate }}</p>
        </div>
      </div>
    </div>

    <button @click="showCommentForm = true" class="pagination-button">Add Comment</button>

    <div v-if="showCommentForm" class="comment-form">
      <h2>Add Comment</h2>
      <form @submit.prevent="submitComment">
        <label for="content">Comment:</label>
        <textarea id="content" v-model="commentForm.content" required></textarea>
        <label for="rating">Rating:</label>
        <input type="number" id="rating" v-model="commentForm.rating" min="0" max="5" required />
        <button type="submit" class="pagination-button">Submit Comment</button>
      </form>
    </div>

    <h2>Comments</h2>
    <ul>
      <li v-for="comment in paginatedComments" :key="comment.id">
        <div class="comment">
          <p><strong>{{ comment.username }}:</strong> {{ comment.content }} (Rating: {{ comment.rating }})</p>
          <p><em>Posted on: {{ new Date(comment.createdAt).toLocaleString() }}</em></p>
          <button v-if="isCommentOwner(comment)" @click="editComment(comment)" class="pagination-button">Edit</button>
          <button v-if="isCommentOwner(comment)" @click="deleteComment(comment.id)" class="pagination-button">Delete</button>
        </div>
      </li>
    </ul>

    <div v-if="editingComment" class="edit-comment-form">
      <h2>Edit Comment</h2>
      <form @submit.prevent="updateComment">
        <label for="editContent">Comment:</label>
        <textarea id="editContent" v-model="editingComment.content" required></textarea>
        <label for="editRating">Rating:</label>
        <input type="number" id="editRating" v-model="editingComment.rating" min="0" max="5" required />
        <button type="submit" class="pagination-button">Update Comment</button>
        <button @click="cancelEdit" class="pagination-button">Cancel</button>
      </form>
    </div>

    <div class="pagination">
      <button @click="prevPage" :disabled="currentPage === 1" class="pagination-button">Previous</button>
      <button
        v-for="page in totalPages"
        :key="page"
        @click="goToPage(page)"
        :class="{ active: currentPage === page }"
        class="pagination-button"
      >{{ page }}</button>
      <button @click="nextPage" :disabled="currentPage === totalPages" class="pagination-button">Next</button>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'CampingSpotProfile',
  data() {
    return {
      campingSpot: {},
      comments: [],
      showCommentForm: false,
      commentForm: {
        content: '',
        rating: 0,
      },
      editingComment: null,
      currentPage: 1,
      itemsPerPage: 6,
    };
  },
  computed: {
    paginatedComments() {
      const start = (this.currentPage - 1) * this.itemsPerPage;
      return this.comments.slice(start, start + this.itemsPerPage);
    },
    totalPages() {
      return Math.ceil(this.comments.length / this.itemsPerPage);
    }
  },
  methods: {
    async fetchCampingSpot() {
      const spotId = this.$route.params.id;
      try {
        const response = await axios.get(`http://localhost:5000/api/CampingSpots/${spotId}`);
        this.campingSpot = response.data;
      } catch (error) {
        console.error('Error fetching camping spot:', error);
      }
    },
    async fetchComments() {
      const spotId = this.$route.params.id;
      try {
        const response = await axios.get(`http://localhost:5000/api/comments/campingspot/${spotId}`);
        this.comments = response.data;
      } catch (error) {
        console.error('Error fetching comments:', error);
      }
    },
    async submitComment() {
      const spotId = this.$route.params.id;
      const user = JSON.parse(localStorage.getItem('user'));
      if (!user) {
        console.error('User not authenticated.');
        return;
      }
      const userId = user.userId;
      const username = "user.username";

      try {
        const commentData = {
          content: this.commentForm.content,
          rating: this.commentForm.rating,
          userId,
          username,
          campingSpotId: spotId,
        };
        await axios.post('http://localhost:5000/api/comments', commentData, {
          headers: {
            'Content-Type': 'application/json',
          },
        });
        this.showCommentForm = false;
        this.commentForm.content = '';
        this.commentForm.rating = 0;
        await this.fetchComments();
      } catch (error) {
        console.error('Error adding comment:', error.response ? error.response.data : error.message);
      }
    },
    isCommentOwner(comment) {
      const user = JSON.parse(localStorage.getItem('user'));
      return user && comment.userId === user.userId;
    },
    async deleteComment(commentId) {
      try {
        await axios.delete(`http://localhost:5000/api/comments/${commentId}`);
        await this.fetchComments();
      } catch (error) {
        console.error('Error deleting comment:', error);
      }
    },
    editComment(comment) {
      this.editingComment = { ...comment };
    },
    cancelEdit() {
      this.editingComment = null;
    },
    async updateComment() {
      try {
        await axios.put(`http://localhost:5000/api/comments/${this.editingComment.id}`, this.editingComment, {
          headers: {
            'Content-Type': 'application/json',
          },
        });
        this.editingComment = null;
        await this.fetchComments();
      } catch (error) {
        console.error('Error updating comment:', error);
      }
    },
    prevPage() {
      if (this.currentPage > 1) {
        this.currentPage--;
      }
    },
    nextPage() {
      if (this.currentPage < this.totalPages) {
        this.currentPage++;
      }
    },
    goToPage(page) {
      this.currentPage = page;
    }
  },
  async mounted() {
    await this.fetchCampingSpot();
    await this.fetchComments();
  },
};
</script>

<style scoped>
.camping-spot-profile {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 20px;
}

.card {
  width: 80%;
  display: flex;
  justify-content: space-between;
  padding: 20px;
  margin-bottom: 20px;
  border: 1px solid #ccc;
  border-radius: 10px;
  background-color: #f9f9f9;
}

.card-body {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
}

.image-section {
  width: 40%;
}

.image-section img {
  max-width: 100%;
  height: auto;
  border-radius: 10px;
}

.details-section {
  width: 55%;
  margin-left: 20px;
}

.comment-form, .edit-comment-form {
  width: 80%;
  margin-bottom: 20px;
}

.comment-form textarea, .edit-comment-form textarea {
  width: 100%;
  height: 100px;
}

.comments-section {
  width: 80%;
  margin-bottom: 20px;
}

.comment {
  margin-top: 10px;
  padding: 10px;
  border: 1px solid #ccc;
  border-radius: 5px;
  background-color: #f9f9f9;
}

.pagination {
  display: flex;
  justify-content: center;
  margin-top: 20px;
}

.pagination-button {
  background-color: green;
  color: white;
  border: none;
  padding: 10px 20px;
  margin: 0 5px;
  border-radius: 5px;
  cursor: pointer;
}

.pagination-button:disabled {
  background-color: #ccc;
  cursor: not-allowed;
}

.pagination-button.active {
  background-color: darkgreen;
  font-weight: bold;
}
</style>
